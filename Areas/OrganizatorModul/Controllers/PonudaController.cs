using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.OrganizatorModul.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: false, client: false)]
    [Area("OrganizatorModul")]
    public class PonudaController : Controller
    {
        private readonly MojContext _context;
        public PonudaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraBrojaPonude(string brojPonude)
        {
            if (_context.Ugovori.Any(a => a.BrojUgovora.ToUpper() == brojPonude.ToUpper()) ||
                _context.Ponude.Any(a => a.BrojPonude.ToUpper() == brojPonude.ToUpper()) ||
                _context.Izvjestaji.Any(a => a.BrojIzvjestaja.ToUpper() == brojPonude.ToUpper()))
            {
                return Json($"Broj ponude \"{brojPonude}\" se već nalazi u bazi");
            }
            return Json(true);
        }

        private void GenerisiKlijente(PonudaDodajVM model)
        {
            model.Klijenti = _context.Klijenti.Where(w => w.KlijentStatus == true).Select(s => new SelectListItem
            {
                Value = s.KlijentId.ToString(),
                Text = s.SkraceniNaziv + " - " + s.IdBroj
            }).ToList();
        }

        public IActionResult PonudaPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index()
        {
            var model = new PonudaIndexVM
            {
                Rows = _context.Ponude.Select(s => new PonudaIndexVM.Row
                {
                    PonudaId = s.PonudaId,
                    BrojPonude = s.BrojPonude,
                    DatumIzdavanja = s.DatumIzdavanja.Date.ToString("dd.MM.yyyy"),
                    NazivKlijenta = s.Klijent.Naziv,
                    UkupanIznosSaPDV = s.UkupanIznosSaPdv,
                    PonudaStatus = s.PonudaStatus
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new PonudaDodajVM
            {
                DatumIzdavanja = DateTime.Now.Date,
                PonudaStatus = true,
                UkupanIznosBezPdv = 1,
                PDV = 17
            };

            GenerisiKlijente(model);

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var ponuda = _context.Ponude.Where(w => w.PonudaId == id).Include(i => i.Klijent).ThenInclude(t => t.Mjesto).Include(n => n.Korisnik).SingleOrDefault();

            if (ponuda == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new PonudaDetaljiVM
            {
                BrojPonude = ponuda.BrojPonude,
                NazivKlijenta = ponuda.Klijent.Naziv + ", ul. " + ponuda.Klijent.Ulica + ", " + ponuda.Klijent.Mjesto.Naziv,
                DatumIzdavanja = ponuda.DatumIzdavanja.ToString("dd.MM.yyyy"),
                PonudaStatus = ponuda.PonudaStatus,
                Korisnik = ponuda.Korisnik.Ime + " " + ponuda.Korisnik.Prezime,
                UkupanIznosBezPdv = ponuda.UkupanIznosBezPdv,
                PDV = ponuda.UkupanIznosSaPdv - ponuda.UkupanIznosBezPdv,
                UkupanIznosSaPdv = ponuda.UkupanIznosSaPdv,
                PonudaPath = ponuda.PonudaPath
            };

            return View(model);
        }

        public IActionResult Snimi(PonudaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiKlijente(input);
                return View("Dodaj", input);
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            Ponuda novaPonuda = new Ponuda
            {
                BrojPonude = input.BrojPonude,
                DatumIzdavanja = input.DatumIzdavanja,
                UkupanIznosBezPdv = input.UkupanIznosBezPdv,
                UkupanIznosSaPdv = input.UkupanIznosBezPdv * ((input.PDV / 100) + 1),
                PonudaStatus = input.PonudaStatus,
                KlijentId = input.KlijentId,
                KorisnikId = korisnik.KorisnikId
            };

            novaPonuda.PonudaPath = UploadDokumenata.UploadDoc(input.Ponuda, input.BrojPonude, UploadDokumenata.TipoviDokumenata.Ponude);

            _context.Ponude.Add(novaPonuda);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var ponuda = _context.Ponude.Find(id);

            if (ponuda == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            UploadDokumenata.DeleteDoc(ponuda.PonudaPath);

            _context.Ponude.Remove(ponuda);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}