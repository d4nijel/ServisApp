using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.KlijentModul.ViewModels;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.KlijentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: false, client: true)]
    [Area("KlijentModul")]
    public class PregledObjekataController : Controller
    {
        private readonly MojContext _context;
        public PregledObjekataController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraEmailaObjekta(string kontaktEmail, int objekatId)
        {
            if (objekatId == 0)
            {
                if (_context.Objekti.Any(a => a.KontaktEmail.ToUpper() == kontaktEmail.ToUpper()))
                {
                    return Json($"Email adresa \"{kontaktEmail}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Objekti.Any(a => a.KontaktEmail.ToUpper() == kontaktEmail.ToUpper() && a.ObjekatId != objekatId))
                {
                    return Json($"Email adresa \"{kontaktEmail}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            AutentifikacijaVM logiraniKorisnik = HttpContext.GetLogiraniKorisnik();
            var klijentskiRacun = _context.KlijentskiRacuni.Find(logiraniKorisnik.KlijentskiRacunId);

            var model = new ObjekatIndexVM
            {
                Rows = _context.Objekti.Where(w => w.KlijentId == klijentskiRacun.KlijentId && w.ObjekatStatus == true).Select(s => new ObjekatIndexVM.Row
                {
                    ObjekatId = s.ObjekatId,
                    Naziv = s.Naziv,
                    Lokacija = "ul. " + s.Ulica + ", " + s.Mjesto.Naziv,
                    BrojRadnihNaloga = s.RadniNalozi.Count(),
                    KontaktOsoba = s.KontaktOsoba,
                    KontaktMobitel = s.KontaktBrojMobitel
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            AutentifikacijaVM logiraniKorisnik = HttpContext.GetLogiraniKorisnik();
            var klijentskiRacun = _context.KlijentskiRacuni.Find(logiraniKorisnik.KlijentskiRacunId);

            var objekat = _context.Objekti.Where(w => w.ObjekatId == id).Include(i => i.Klijent).Include(n => n.Mjesto).ThenInclude(t => t.Opcina).SingleOrDefault();

            if (objekat == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            if (klijentskiRacun.KlijentId != objekat.KlijentId)
            {
                TempData["error_poruka"] = "Nemate pravo pristupa";
                return RedirectToAction("Index", "Autentifikacija", new { @area = "" });
            }

            var model = new ObjekatEditVM()
            {
                ObjekatId = objekat.ObjekatId,
                Naziv = objekat.Naziv,
                Ulica = objekat.Ulica,
                NazivMjesta = objekat.Mjesto.Naziv + ", općina " + objekat.Mjesto.Opcina.Naziv,
                KontaktOsoba = objekat.KontaktOsoba,
                KontaktBrojFiksni = objekat.KontaktBrojFiksni,
                KontaktBrojMobitel = objekat.KontaktBrojMobitel,
                KontaktEmail = objekat.KontaktEmail
            };

            return View(model);
        }

        public IActionResult Edit(ObjekatEditVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Detalji", input);
            }

            var stariObjekat = _context.Objekti.Find(input.ObjekatId);

            if (stariObjekat != null)
            {
                stariObjekat.KontaktOsoba = input.KontaktOsoba;
                stariObjekat.KontaktBrojFiksni = input.KontaktBrojFiksni;
                stariObjekat.KontaktBrojMobitel = input.KontaktBrojMobitel;
                stariObjekat.KontaktEmail = input.KontaktEmail;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}