using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.MenadzmentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.MenadzmentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: true, client: false)]
    [Area("MenadzmentModul")]
    public class UgovorController : Controller
    {
        private readonly MojContext _context;
        public UgovorController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraBrojaUgovora(string brojUgovora)
        {
            if (_context.Ugovori.Any(a => a.BrojUgovora.ToUpper() == brojUgovora.ToUpper()) ||
                _context.Ponude.Any(a => a.BrojPonude.ToUpper() == brojUgovora.ToUpper()) ||
                _context.Izvjestaji.Any(a => a.BrojIzvjestaja.ToUpper() == brojUgovora.ToUpper()))
            {
                return Json($"Broj ugovora \"{brojUgovora}\" se već nalazi u bazi");
            }
            return Json(true);
        }

        private void GenerisiKlijente(UgovorDodajVM model)
        {
            model.Klijenti = _context.Klijenti.Where(w => w.KlijentStatus == true).Select(s => new SelectListItem
            {
                Value = s.KlijentId.ToString(),
                Text = s.SkraceniNaziv + " - " + s.IdBroj
            }).ToList();
        }

        public IActionResult UgovorPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index()
        {
            var ugovori = _context.Ugovori.Where(w => w.UgovorStatus == true && DateTime.Now.Date >= w.DatumIsteka.Date).ToList();

            foreach (var item in ugovori)
            {
                item.UgovorStatus = false;
            }
            _context.SaveChanges();

            var model = new UgovorIndexVM()
            {
                Rows = _context.Ugovori.Select(s => new UgovorIndexVM.Row
                {
                    UgovorId = s.UgovorId,
                    BrojUgovora = s.BrojUgovora,
                    Naziv = s.Naziv,
                    Klijent = s.Klijent.Naziv,
                    DatumPotpisivanja = s.DatumPotpisivanja.Date.ToString("dd.MM.yyyy"),
                    UgovorStatus = s.UgovorStatus
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new UgovorDodajVM
            {
                DatumPotpisivanja = DateTime.Now.Date,
                DatumIsteka = DateTime.Now.Date.AddYears(3)
            };

            GenerisiKlijente(model);

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var ugovor = _context.Ugovori.Where(w => w.UgovorId == id).Include(i => i.Klijent).ThenInclude(t => t.Mjesto).SingleOrDefault();

            if (ugovor == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new UgovorDetaljiVM
            {
                BrojUgovora = ugovor.BrojUgovora,
                Naziv = ugovor.Naziv,
                DatumPotpisivanja = ugovor.DatumPotpisivanja.ToString("dd.MM.yyyy"),
                DatumIsteka = ugovor.DatumIsteka.ToString("dd.MM.yyyy"),
                UgovorPath = ugovor.UgovorPath,
                UgovorStatus = ugovor.UgovorStatus,
                Klijent = ugovor.Klijent.Naziv + ", ul. " + ugovor.Klijent.Ulica + ", " + ugovor.Klijent.Mjesto.Naziv
            };

            return View(model);
        }

        public IActionResult Snimi(UgovorDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiKlijente(input);
                return View("Dodaj", input);
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            Ugovor noviUgovor = new Ugovor
            {
                BrojUgovora = input.BrojUgovora,
                Naziv = input.Naziv,
                DatumPotpisivanja = input.DatumPotpisivanja,
                DatumIsteka = input.DatumIsteka,
                UgovorStatus = true,
                KlijentId = input.KlijentId,
                KorisnikId = korisnik.KorisnikId
            };

            noviUgovor.UgovorPath = UploadDokumenata.UploadDoc(input.Ugovor, input.BrojUgovora, UploadDokumenata.TipoviDokumenata.Ugovori);

            _context.Ugovori.Add(noviUgovor);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var ugovor = _context.Ugovori.Find(id);

            if (ugovor == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            UploadDokumenata.DeleteDoc(ugovor.UgovorPath);

            _context.Ugovori.Remove(ugovor);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}