using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.InzinjerModul.ViewModels;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: true, client: true)]
    public class IzvrsenaIspitivanjaController : Controller
    {
        private readonly MojContext _context;
        public IzvrsenaIspitivanjaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult RadniNalogPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult IzvjestajPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index(int id)
        {
            var objekat = _context.Objekti.Find(id);

            if (objekat == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new IzvrsenaIspitivanjaIndexVM
            {
                Rows = _context.Ispitivanja.Where(w => w.RadniNalog.ObjekatId == objekat.ObjekatId).Select(s => new IzvrsenaIspitivanjaIndexVM.Row
                {
                    IspitivanjeId = s.IspitivanjeId,
                    NazivIspitivanja = s.NazivIspitivanja.Oznaka,
                    BrojRadnogNaloga = s.RadniNalog.BrojRadnogNaloga,
                    RadniNalogId = s.RadniNalogId,
                    DatumIspitivanja = s.DatumIspitivanja.ToString("dd.MM.yyyy"),
                    DatumNarednogIspitivanja = s.DatumNarednogIspitivanja.ToString("dd.MM.yyyy"),
                    BroDanaDoNarednogIspitivanja = s.DatumNarednogIspitivanja.Date.Subtract(DateTime.Now.Date).Days,
                    TipIspitivanja = s.TipIspitivanja,
                    PostojanjeIzvjestaja = (s.Izvjestaj == null) ? false : true,
                    IzvjestajId = (s.Izvjestaj != null) ? s.Izvjestaj.IzvjestajId : 0
                }).ToList()
            };

            return PartialView(model);
        }

        public IActionResult DetaljiObjekta(int id)
        {
            var objekat = _context.Objekti.Where(w => w.ObjekatId == id).Include(i => i.Klijent).Include(n => n.Mjesto).ThenInclude(t => t.Opcina).SingleOrDefault();

            if (objekat == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            if (korisnik.IsKlijent)
            {
                var klijentskiRacun = _context.KlijentskiRacuni.Find(korisnik.KlijentskiRacunId);

                if (klijentskiRacun != null)
                {
                    if (objekat.KlijentId != klijentskiRacun.KlijentId)
                    {
                        TempData["error_poruka"] = "Nemate pravo pristupa";
                        return RedirectToAction("Index", "Autentifikacija");
                    }
                }
            }

            var model = new ObjekatDetaljiVM()
            {
                ObjekatId = objekat.ObjekatId,
                Naziv = objekat.Naziv,
                Ulica = objekat.Ulica,
                KontaktOsoba = objekat.KontaktOsoba,
                KontaktBrojFiksni = objekat.KontaktBrojFiksni,
                KontaktBrojMobitel = objekat.KontaktBrojMobitel,
                KontaktEmail = objekat.KontaktEmail,
                ObjekatStatus = objekat.ObjekatStatus,
                NazivKlijenta = objekat.Klijent.SkraceniNaziv,
                NazivMjesta = objekat.Mjesto.Naziv
            };

            return View(model);
        }

        public IActionResult DetaljiIzvjestaja(int id)
        {
            var ispitivanje = _context.Ispitivanja.Where(w => w.IspitivanjeId == id).Include(i => i.Izvjestaj).ThenInclude(t => t.Korisnik).Include(n => n.NazivIspitivanja).Include(c => c.RadniNalog).ThenInclude(h => h.Objekat).ThenInclude(e => e.Klijent).SingleOrDefault();

            if (ispitivanje == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            if (korisnik.IsKlijent)
            {
                var klijentskiRacun = _context.KlijentskiRacuni.Find(korisnik.KlijentskiRacunId);

                if (klijentskiRacun != null)
                {
                    if (ispitivanje.RadniNalog.Objekat.Klijent.KlijentId != klijentskiRacun.KlijentId)
                    {
                        TempData["error_poruka"] = "Nemate pravo pristupa";
                        return RedirectToAction("Index", "Autentifikacija");
                    }
                }
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
                NazivIspitivanjaOznaka = ispitivanje.NazivIspitivanja.Oznaka,
                ObjekatId = ispitivanje.RadniNalog.ObjekatId
            };

            return PartialView(model);
        }

        public IActionResult DetaljiIspitivanja(int id)
        {
            var ispitivanje = _context.Ispitivanja.Where(w => w.IspitivanjeId == id).Include(i => i.Izvjestaj).ThenInclude(t => t.Korisnik).Include(n => n.NazivIspitivanja).Include(c => c.RadniNalog).ThenInclude(h => h.Objekat).ThenInclude(e => e.Klijent).SingleOrDefault();

            if (ispitivanje == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            if (korisnik.IsKlijent)
            {
                var klijentskiRacun = _context.KlijentskiRacuni.Find(korisnik.KlijentskiRacunId);

                if (klijentskiRacun != null)
                {
                    if (ispitivanje.RadniNalog.Objekat.Klijent.KlijentId != klijentskiRacun.KlijentId)
                    {
                        TempData["error_poruka"] = "Nemate pravo pristupa";
                        return RedirectToAction("Index", "Autentifikacija");
                    }
                }
            }

            var model = new IspitivanjeDetaljiVM()
            {
                RadniNalogId = ispitivanje.RadniNalogId,
                DatumIspitivanja = ispitivanje.DatumIspitivanja.Date.ToString("dd.MM.yyyy"),
                DatumNarednogIspitivanja = ispitivanje.DatumNarednogIspitivanja.Date.ToString("dd.MM.yyyy"),
                BrojDanaDoNarednogIspitivanja = ispitivanje.DatumNarednogIspitivanja.Date.Subtract(DateTime.Now.Date).Days.ToString(),
                TipIspitivanja = ispitivanje.TipIspitivanja,
                Napomena = ispitivanje.Napomena,
                NazivIspitivanja = ispitivanje.NazivIspitivanja.Naziv,
                NazivIspitivanjaOznaka = ispitivanje.NazivIspitivanja.Oznaka,
                ObjekatId = ispitivanje.RadniNalog.ObjekatId
            };

            return PartialView(model);
        }

        public IActionResult DetaljiRadnogNaloga(int id)
        {
            var radniNalog = _context.RadniNalozi.Where(w => w.RadniNalogId == id).Include(i => i.Objekat).ThenInclude(t => t.Klijent).ThenInclude(l => l.Mjesto).Include(c => c.Objekat).ThenInclude(h => h.Mjesto).SingleOrDefault();

            if (radniNalog == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            if (korisnik.IsKlijent)
            {
                var klijentskiRacun = _context.KlijentskiRacuni.Find(korisnik.KlijentskiRacunId);

                if (klijentskiRacun != null)
                {
                    if (radniNalog.Objekat.Klijent.KlijentId != klijentskiRacun.KlijentId)
                    {
                        TempData["error_poruka"] = "Nemate pravo pristupa";
                        return RedirectToAction("Index", "Autentifikacija");
                    }
                }
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
                NazivKlijenta = radniNalog.Objekat.Klijent.Naziv + ", ul. " + radniNalog.Objekat.Klijent.Ulica + ", " + radniNalog.Objekat.Klijent.Mjesto.Naziv,
                ObjekatId = radniNalog.ObjekatId
            };

            var clanoviServisa = _context.ObavljeniPoslovi.Where(w => w.RadniNalogId == radniNalog.RadniNalogId).Select(s => s.ClanServisa).ToList();

            if (clanoviServisa != null)
            {
                model.ClanoviServisa = new List<string>();
                foreach (var item in clanoviServisa)
                {
                    model.ClanoviServisa.Add(item.Zanimanje + " - " + item.Ime + " " + item.Prezime);
                }
            }

            return PartialView(model);
        }
    }
}