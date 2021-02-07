using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.InzinjerModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.InzinjerModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: true, mgmt: false, client: false)]
    [Area("InzinjerModul")]
    public class IzvjestajController : Controller
    {
        private readonly MojContext _context;
        public IzvjestajController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraBrojaIzvjestaja(string brojIzvjestaja)
        {
            if (_context.Ugovori.Any(a => a.BrojUgovora.ToUpper() == brojIzvjestaja.ToUpper()) ||
                _context.Ponude.Any(a => a.BrojPonude.ToUpper() == brojIzvjestaja.ToUpper()) ||
                _context.Izvjestaji.Any(a => a.BrojIzvjestaja.ToUpper() == brojIzvjestaja.ToUpper()))
            {
                return Json($"Broj izvještaja \"{brojIzvjestaja}\" se već nalazi u bazi");
            }
            return Json(true);
        }

        public IActionResult IzvjestajPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Dodaj(int id)
        {
            var ispitivanje = _context.Ispitivanja.Where(w => w.IspitivanjeId == id).Include(i => i.NazivIspitivanja).SingleOrDefault();

            if (ispitivanje == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new IzvjestajDodajVM()
            {
                RadniNalogId = ispitivanje.RadniNalogId,
                IspitivanjeId = ispitivanje.IspitivanjeId,
                DatumKreiranja = DateTime.Now.Date,
                IzvjestajStatus = true,
                NazivIspitivanja = ispitivanje.NazivIspitivanja.Naziv,
                NazivIspitivanjaOznaka = ispitivanje.NazivIspitivanja.Oznaka
            };

            return PartialView(model);
        }

        public IActionResult Detalji(int id)
        {
            var ispitivanje = _context.Ispitivanja.Where(w => w.IspitivanjeId == id).Include(i => i.Izvjestaj).ThenInclude(t => t.Korisnik).Include(n => n.NazivIspitivanja).SingleOrDefault();

            if (ispitivanje == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new IzvjestajDetaljiVM()
            {
                RadniNalogId = ispitivanje.RadniNalogId,
                BrojIzvjestaja = ispitivanje.Izvjestaj.BrojIzvjestaja,
                DatumKreiranja = ispitivanje.Izvjestaj.DatumKreiranja.Date.ToString("dd.MM.yyyy"),
                IzvjestajPath = ispitivanje.Izvjestaj.IzvjestajPath,
                IzvjestajStatus = (ispitivanje.Izvjestaj.IzvjestajStatus == true) ? "Prošao" : "Nije prošao",
                Korisnik = ispitivanje.Izvjestaj.Korisnik.Ime + " " + ispitivanje.Izvjestaj.Korisnik.Prezime,
                NazivIspitivanja = ispitivanje.NazivIspitivanja.Naziv,
                NazivIspitivanjaOznaka = ispitivanje.NazivIspitivanja.Oznaka
            };

            return PartialView(model);
        }

        public IActionResult Snimi(IzvjestajDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Dodaj", input);
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            Izvjestaj noviIzvjestaj = new Izvjestaj()
            {
                BrojIzvjestaja = input.BrojIzvjestaja,
                DatumKreiranja = input.DatumKreiranja.Date,
                IzvjestajStatus = input.IzvjestajStatus,
                IspitivanjeId = input.IspitivanjeId,
                KorisnikId = korisnik.KorisnikId
            };

            noviIzvjestaj.IzvjestajPath = UploadDokumenata.UploadDoc(input.Izvjestaj, input.BrojIzvjestaja, UploadDokumenata.TipoviDokumenata.Izvjestaji);

            _context.Izvjestaji.Add(noviIzvjestaj);
            _context.SaveChanges();

            return Redirect("/InzinjerModul/Ispitivanje/Index?Id=" + input.RadniNalogId);
        }

        public IActionResult Obrisi(int id)
        {
            var izvjestaj = _context.Izvjestaji.Where(w => w.IzvjestajId == id).Include(i => i.Ispitivanje).SingleOrDefault();

            if (izvjestaj == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            UploadDokumenata.DeleteDoc(izvjestaj.IzvjestajPath);

            _context.Izvjestaji.Remove(izvjestaj);
            _context.SaveChanges();

            return Redirect("/InzinjerModul/Ispitivanje/Index?Id=" + izvjestaj.Ispitivanje.RadniNalogId);
        }
    }
}