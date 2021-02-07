using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.KlijentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.KlijentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: false, client: true)]
    [Area("KlijentModul")]
    public class KontaktController : Controller
    {
        private readonly MojContext _context;
        public KontaktController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraEmailaKlijenta(string kontaktEmail, int klijentId)
        {
            if (klijentId == 0)
            {
                if (_context.Klijenti.Any(a => a.KontaktEmail.ToUpper() == kontaktEmail.ToUpper()))
                {
                    return Json($"Email adresa \"{kontaktEmail}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Klijenti.Any(a => a.KontaktEmail.ToUpper() == kontaktEmail.ToUpper() && a.KlijentId != klijentId))
                {
                    return Json($"Email adresa \"{kontaktEmail}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Detalji()
        {
            AutentifikacijaVM logiraniKorisnik = HttpContext.GetLogiraniKorisnik();

            var klijentskiRacun = _context.KlijentskiRacuni.Find(logiraniKorisnik.KlijentskiRacunId);
            var klijent = _context.Klijenti.Where(w => w.KlijentId == klijentskiRacun.KlijentId).Include(i => i.Mjesto).ThenInclude(t => t.Opcina).SingleOrDefault();

            var model = new KlijentUrediVM
            {
                KlijentId = klijent.KlijentId,
                Naziv = klijent.Naziv,
                IdBroj = klijent.IdBroj,
                Ulica = klijent.Ulica,
                Sjediste = klijent.Mjesto.Naziv + ", općina " + klijent.Mjesto.Opcina.Naziv,
                KontaktOsoba = klijent.KontaktOsoba,
                KontaktBrojFiksni = klijent.KontaktBrojFiksni,
                KontaktBrojMobitel = klijent.KontaktBrojMobitel,
                KontaktEmail = klijent.KontaktEmail
            };

            return View(model);
        }

        public IActionResult Edit(KlijentUrediVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Detalji", input);
            }

            var stariKlijent = _context.Klijenti.Find(input.KlijentId);

            if (stariKlijent != null)
            {
                stariKlijent.KontaktOsoba = input.KontaktOsoba;
                stariKlijent.KontaktBrojFiksni = input.KontaktBrojFiksni;
                stariKlijent.KontaktBrojMobitel = input.KontaktBrojMobitel;
                stariKlijent.KontaktEmail = input.KontaktEmail;

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home", new { @area = "" });
        }
    }
}