using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.KlijentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.OrganizatorModul.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: false, client: false)]
    [Area("OrganizatorModul")]
    public class DodijeliZahtjevController : Controller
    {
        private readonly MojContext _context;
        public DodijeliZahtjevController(MojContext context)
        {
            _context = context;
        }

        private void GenerisiKorisnike(ZahtjevDetaljiVM model)
        {
            int UlogaOrg = 2; //Organizator
            int UlogaIng = 3; //Inžinjer

            var dozvoljeniKorisniciIds = _context.Permisije.Where(w => w.PermisijaStatus == true && (w.UlogaId == UlogaOrg || w.UlogaId == UlogaIng)).Select(s => s.KorisnikId).Distinct().ToList();

            var korisnici = new List<Korisnik>();

            foreach (var korisnikId in dozvoljeniKorisniciIds)
            {
                var k = _context.Korisnici.Find(korisnikId);

                if (k != null)
                {
                    korisnici.Add(k);
                }
            }

            model.Korisnici = korisnici.Where(w => w.KorisnikStatus == true).Select(s => new SelectListItem
            {
                Value = s.KorisnikId.ToString(),
                Text = s.Ime + " " + s.Prezime
            }).ToList();
        }

        //dodijeli zahtjeve
        public IActionResult Index()
        {
            //samo OTVORENI ZAHTJEVI
            var model = new ZahtjevIndexVM
            {
                Rows = _context.Zahtjevi.Where(w => w.ZahtjevStatusId != 3).Select(s => new ZahtjevIndexVM.Row //može se dodijeliti više puta
                {
                    ZahtjevId = s.ZahtjevId,
                    Naslov = s.Naslov,
                    DatumKreiranja = s.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                    KategorijaZahtjeva = s.ZahtjevKategorija.Naziv,
                    StatusZahtjevaId = s.ZahtjevStatus.ZahtjevStatusId,
                    StatusZahtjeva = s.ZahtjevStatus.Naziv,
                    Korisnik = (s.Korisnik != null) ? s.Korisnik.Ime + " " + s.Korisnik.Prezime : "Nije dodijeljen korisnik",
                    KlijentskiRacun = s.KlijentskiRacun.Ime + " " + s.KlijentskiRacun.Prezime,
                    Klijent = s.KlijentskiRacun.Klijent.SkraceniNaziv
                }).ToList()
            };
            model.Dodijeli = true;

            return View("~/Views/Podrska/Index.cshtml", model);
        }

        public IActionResult Dodijeli(int id)
        {
            var zahtjev = _context.Zahtjevi.Where(w => w.ZahtjevId == id).Include(i => i.KlijentskiRacun).ThenInclude(t => t.Klijent).Include(n => n.ZahtjevKategorija).Include(c => c.ZahtjevStatus).SingleOrDefault();

            if (zahtjev == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ZahtjevDetaljiVM()
            {
                ZahtjevId = zahtjev.ZahtjevId,
                Naslov = zahtjev.Naslov,
                Opis = zahtjev.Opis,
                DatumKreiranja = zahtjev.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                KlijentskiRacun = zahtjev.KlijentskiRacun.Ime + " " + zahtjev.KlijentskiRacun.Prezime,
                ZahtjevKategorija = zahtjev.ZahtjevKategorija.Naziv,
                StatusZahtjevaId = zahtjev.ZahtjevStatusId,
                StatusZahtjevaNaziv = zahtjev.ZahtjevStatus.Naziv,
                Klijent = zahtjev.KlijentskiRacun.Klijent.SkraceniNaziv
            };

            if (zahtjev.KorisnikId != null)
            {
                model.KorisnikId = zahtjev.KorisnikId.Value;
            }

            GenerisiKorisnike(model);

            return View(model);
        }

        public IActionResult SnimiDodjelu(ZahtjevDetaljiVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiKorisnike(input);
                return View("Dodijeli", input);
            }

            var stariZahtjev = _context.Zahtjevi.Find(input.ZahtjevId);

            if (stariZahtjev != null)
            {
                stariZahtjev.KorisnikId = input.KorisnikId;
                stariZahtjev.ZahtjevStatusId = 2;//U procesu rješavanja

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}