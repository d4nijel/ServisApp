using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.OrganizatorModul.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: false, client: false)]
    [Area("OrganizatorModul")]
    public class KlijentController : Controller
    {
        private readonly MojContext _context;
        public KlijentController(MojContext context)
        {
            _context = context;
        }

        private void GenerisiMjesto(KlijentDodajVM model)
        {
            model.Mjesto = _context.Mjesta.Select(s => new SelectListItem
            {
                Value = s.MjestoId.ToString(),
                Text = s.Naziv + ", općina " + s.Opcina.Naziv
            }).ToList();
        }

        public IActionResult ProvjeraIdbrojaKlijenta(string idBroj, int klijentId)
        {
            if (klijentId == 0)
            {
                if (_context.Klijenti.Any(a => a.IdBroj == idBroj))
                {
                    return Json($"ID broj \"{idBroj}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Klijenti.Any(a => a.IdBroj == idBroj && a.KlijentId != klijentId))
                {
                    return Json($"ID broj \"{idBroj}\" se već nalazi u bazi");
                }
            }

            return Json(true);
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

        public IActionResult Index()
        {
            var model = new KlijentIndexVM
            {
                Rows = _context.Klijenti.Select(s => new KlijentIndexVM.Row
                {
                    KlijentId = s.KlijentId,
                    Naziv = s.Naziv,
                    IdBroj = s.IdBroj,
                    Lokacija = "ul. " + s.Ulica + ", " + s.Mjesto.Naziv,
                    BrojObjekata = s.Objekti.Count(),
                    KlijentStatus = s.KlijentStatus,
                    PostojanjeProfila = (s.KlijentskiRacun == null) ? false : true,
                    KlijentskiRacunStatus = (s.KlijentskiRacun != null) ? s.KlijentskiRacun.KlijentskiRacunStatus : false,
                    KlijentskiRacunId = (s.KlijentskiRacun != null) ? s.KlijentskiRacun.KlijentskiRacunId : 0,
                    DeleteBtn = (
                    (s.Ugovori.Where(w => w.KlijentId == s.KlijentId).SingleOrDefault()) == null &&
                    (s.Ponuda.Where(w => w.KlijentId == s.KlijentId).SingleOrDefault()) == null &&
                    (s.Objekti.Where(w => w.KlijentId == s.KlijentId).SingleOrDefault()) == null &&
                    ((s.KlijentskiRacun == null) ? true : false)
                    ) ? true : false,
                    ProfilDeleteBtn = (
                    (s.KlijentskiRacun.Zahtjevi.Where(w => w.KlijentskiRacunId == s.KlijentskiRacun.KlijentskiRacunId).SingleOrDefault()) == null &&
                    (s.KlijentskiRacun.Poruke.Where(w => w.KlijentskiRacunId == s.KlijentskiRacun.KlijentskiRacunId).SingleOrDefault()) == null
                    ) ? true : false,
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new KlijentDodajVM();

            GenerisiMjesto(model);

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var klijent = _context.Klijenti.Find(id);

            if (klijent == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new KlijentDodajVM
            {
                KlijentId = klijent.KlijentId,
                Naziv = klijent.Naziv,
                SkraceniNaziv = klijent.SkraceniNaziv,
                IdBroj = klijent.IdBroj,
                Ulica = klijent.Ulica,
                KontaktOsoba = klijent.KontaktOsoba,
                KontaktBrojFiksni = klijent.KontaktBrojFiksni,
                KontaktBrojMobitel = klijent.KontaktBrojMobitel,
                KontaktEmail = klijent.KontaktEmail,
                KlijentStatus = klijent.KlijentStatus,
                MjestoId = klijent.MjestoId
            };

            GenerisiMjesto(model);

            return View(model);
        }

        public IActionResult Snimi(KlijentDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiMjesto(input);
                return View("Dodaj", input);
            }

            Klijent noviKlijent = new Klijent()
            {
                Naziv = input.Naziv,
                SkraceniNaziv = input.SkraceniNaziv,
                IdBroj = input.IdBroj,
                Ulica = input.Ulica,
                KontaktOsoba = input.KontaktOsoba,
                KontaktBrojFiksni = input.KontaktBrojFiksni,
                KontaktBrojMobitel = input.KontaktBrojMobitel,
                KontaktEmail = input.KontaktEmail,
                KlijentStatus = true,
                MjestoId = input.MjestoId
            };

            _context.Klijenti.Add(noviKlijent);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(KlijentDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiMjesto(input);
                return View("Detalji", input);
            }

            var stariKlijent = _context.Klijenti.Find(input.KlijentId);

            if (stariKlijent != null)
            {
                stariKlijent.Naziv = input.Naziv;
                stariKlijent.SkraceniNaziv = input.SkraceniNaziv;
                stariKlijent.IdBroj = input.IdBroj;
                stariKlijent.Ulica = input.Ulica;
                stariKlijent.KontaktOsoba = input.KontaktOsoba;
                stariKlijent.KontaktBrojFiksni = input.KontaktBrojFiksni;
                stariKlijent.KontaktBrojMobitel = input.KontaktBrojMobitel;
                stariKlijent.KontaktEmail = input.KontaktEmail;
                stariKlijent.KlijentStatus = input.KlijentStatus;
                stariKlijent.MjestoId = input.MjestoId;

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var klijent = _context.Klijenti.Find(id);

            if (klijent == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Klijenti.Remove(klijent);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}