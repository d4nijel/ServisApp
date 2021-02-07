using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.InzinjerModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.InzinjerModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: true, mgmt: false, client: false)]
    [Area("InzinjerModul")]
    public class RadniNalogController : Controller
    {
        private readonly MojContext _context;
        public RadniNalogController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraBrojaRadnogNaloga(string brojRadnogNaloga)
        {
            if (_context.RadniNalozi.Any(a => a.BrojRadnogNaloga == brojRadnogNaloga))
            {
                return Json($"Broj radnog naloga \"{brojRadnogNaloga}\" se već nalazi u bazi");
            }

            return Json(true);
        }

        private void GenerisiObjekte(RadniNalogDodajVM model)
        {
            model.Objekti = _context.Objekti.Where(w => w.ObjekatStatus == true).Select(s => new SelectListItem
            {
                Value = s.ObjekatId.ToString(),
                Text = s.Naziv
            }).ToList();
        }

        private void GenerisiClanoveServisa(RadniNalogDodajVM model)
        {
            model.ClanoviServisa = _context.ClanoviServisa.Where(w => w.ClanServisaStatus == true).Select(s => new SelectListItem
            {
                Value = s.ClanServisaId.ToString(),
                Text = s.Zanimanje + " - " + s.Ime + " " + s.Prezime
            }).ToList();
        }

        public IActionResult RadniNalogPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index()
        {
            var model = new RadniNalogIndexVM
            {
                Rows = _context.RadniNalozi.Select(s => new RadniNalogIndexVM.Row
                {
                    RadniNalogId = s.RadniNalogId,
                    BrojRadnogNaloga = s.BrojRadnogNaloga,
                    DatumPocetkaRadova = s.DatumPocetkaRadova.ToString("dd.MM.yyyy HH:mm"),
                    DatumZavrsetkaRadova = s.DatumZavrsetkaRadova.ToString("dd.MM.yyyy HH:mm"),
                    UkupnoSatiRada = s.DatumZavrsetkaRadova.Subtract(s.DatumPocetkaRadova).Hours.ToString() + "h " +
                    s.DatumZavrsetkaRadova.Subtract(s.DatumPocetkaRadova).Minutes.ToString() + "min",
                    NazivKlijenta = s.Objekat.Klijent.SkraceniNaziv,
                    NazivObjekta = s.Objekat.Naziv,
                    BrojIspitivanja = s.Ispitivanja.Where(w => w.RadniNalogId == s.RadniNalogId).Count(),
                    NedostajeIzvjestaja = s.Ispitivanja.Where(w => w.RadniNalogId == s.RadniNalogId).Count()
                    - s.Ispitivanja.Where(w => w.RadniNalogId == s.RadniNalogId).Count(c => c.Izvjestaj != null),
                    DeleteBtn = ((s.Ispitivanja.Where(w => w.RadniNalogId == s.RadniNalogId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new RadniNalogDodajVM
            {
                DatumPocetkaRadova = DateTime.Now.Date.AddHours(8),
                DatumZavrsetkaRadova = DateTime.Now.Date.AddHours(11)
            };

            GenerisiObjekte(model);
            GenerisiClanoveServisa(model);

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var radniNalog = _context.RadniNalozi.Where(w => w.RadniNalogId == id).Include(i => i.Objekat).ThenInclude(t => t.Klijent).ThenInclude(l => l.Mjesto).Include(c => c.Objekat).ThenInclude(h => h.Mjesto).SingleOrDefault();

            if (radniNalog == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new RadniNalogDetaljiVM()
            {
                RadniNalogId = radniNalog.RadniNalogId,
                BrojRadnogNaloga = radniNalog.BrojRadnogNaloga,
                DatumPocetkaRadova = radniNalog.DatumPocetkaRadova.ToString("dd.MM.yyyy HH:mm"),
                DatumZavrsetkaRadova = radniNalog.DatumZavrsetkaRadova.ToString("dd.MM.yyyy HH:mm"),
                UkupnoSatiRada = radniNalog.DatumZavrsetkaRadova.Subtract(radniNalog.DatumPocetkaRadova).Hours.ToString() + "h " +
                radniNalog.DatumZavrsetkaRadova.Subtract(radniNalog.DatumPocetkaRadova).Minutes.ToString() + "min",
                RadniNalogPath = radniNalog.RadniNalogPath,
                NazivObjekta = radniNalog.Objekat.Naziv + ", ul. " + radniNalog.Objekat.Ulica + ", " + radniNalog.Objekat.Mjesto.Naziv,
                NazivKlijenta = radniNalog.Objekat.Klijent.Naziv + ", ul. " + radniNalog.Objekat.Klijent.Ulica + ", " + radniNalog.Objekat.Klijent.Mjesto.Naziv
            };

            var clanoviServisa = _context.ObavljeniPoslovi.Where(w => w.RadniNalogId == radniNalog.RadniNalogId).Select(s => s.ClanServisa).ToList();

            if (clanoviServisa.Count() > 0)
            {
                model.ClanoviServisa = new List<string>();
                foreach (var item in clanoviServisa)
                {
                    model.ClanoviServisa.Add(item.Zanimanje + " - " + item.Ime + " " + item.Prezime);
                }
            }

            return View(model);
        }

        public IActionResult Snimi(RadniNalogDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiObjekte(input);
                GenerisiClanoveServisa(input);
                return View("Dodaj", input);
            }

            RadniNalog noviRadniNalog = new RadniNalog()
            {
                BrojRadnogNaloga = input.BrojRadnogNaloga,
                DatumPocetkaRadova = input.DatumPocetkaRadova,
                DatumZavrsetkaRadova = input.DatumZavrsetkaRadova,
                ObjekatId = input.ObjekatId,
            };

            noviRadniNalog.RadniNalogPath = UploadDokumenata.UploadDoc(input.RadniNalog, input.BrojRadnogNaloga, UploadDokumenata.TipoviDokumenata.RadniNalozi);

            _context.RadniNalozi.Add(noviRadniNalog);
            _context.SaveChanges();

            var ClanoviServisaIds = input.ClanoviServisa.Where(w => w.Selected == true).Select(s => s.Value);

            foreach (var id in ClanoviServisaIds)
            {
                ObavljeniPosao op = new ObavljeniPosao
                {
                    ClanServisaId = int.Parse(id),
                    RadniNalogId = noviRadniNalog.RadniNalogId
                };
                _context.ObavljeniPoslovi.Add(op);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var radniNalog = _context.RadniNalozi.Find(id);

            if (radniNalog == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            UploadDokumenata.DeleteDoc(radniNalog.RadniNalogPath);

            var listaObaljenihPoslova = _context.ObavljeniPoslovi.Where(w => w.RadniNalogId == radniNalog.RadniNalogId);

            foreach (var item in listaObaljenihPoslova)
            {
                _context.ObavljeniPoslovi.Remove(item);
            }
            _context.SaveChanges();

            _context.RadniNalozi.Remove(radniNalog);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}