using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.OrganizatorModul.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: false, client: false)]
    [Area("OrganizatorModul")]
    public class ObjekatController : Controller
    {
        private readonly MojContext _context;
        public ObjekatController(MojContext context)
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

        private void GenerisiMjesto(ObjekatDodajVM model)
        {
            model.Mjesto = _context.Mjesta.Select(s => new SelectListItem
            {
                Value = s.MjestoId.ToString(),
                Text = s.Naziv + ", općina " + s.Opcina.Naziv
            }).ToList();
        }

        private void GenerisiKlijenta(ObjekatDodajVM model)
        {
            model.Klijent = _context.Klijenti.Where(w => w.KlijentStatus == true).Select(s => new SelectListItem
            {
                Value = s.KlijentId.ToString(),
                Text = s.SkraceniNaziv + " - " + s.IdBroj
            }).ToList();
        }

        public IActionResult Index()
        {
            var model = new ObjekatIndexVM
            {
                Rows = _context.Objekti.Select(s => new ObjekatIndexVM.Row
                {
                    ObjekatId = s.ObjekatId,
                    Naziv = s.Naziv,
                    Lokacija = "ul. " + s.Ulica + ", " + s.Mjesto.Naziv,
                    Klijent = s.Klijent.SkraceniNaziv,
                    BrojRadnihNaloga = s.RadniNalozi.Count(),
                    ObjekatStatus = s.ObjekatStatus,
                    DeleteBtn = ((s.RadniNalozi.Where(w => w.ObjekatId == s.ObjekatId).SingleOrDefault()) == null) ? true : false,
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new ObjekatDodajVM();

            GenerisiMjesto(model);
            GenerisiKlijenta(model);

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var objekat = _context.Objekti.Where(w => w.ObjekatId == id).Include(i => i.Klijent).Include(n => n.Mjesto).ThenInclude(t => t.Opcina).SingleOrDefault();

            if (objekat == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ObjekatUrediVM()
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
                NazivMjesta = objekat.Mjesto.Naziv + ", općina " + objekat.Mjesto.Opcina.Naziv
            };

            return View(model);
        }

        public IActionResult Snimi(ObjekatDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiMjesto(input);
                GenerisiKlijenta(input);
                return View("Dodaj", input);
            }

            var noviObjekat = new Objekat()
            {
                Naziv = input.Naziv,
                Ulica = input.Ulica,
                KontaktOsoba = input.KontaktOsoba,
                KontaktBrojFiksni = input.KontaktBrojFiksni,
                KontaktBrojMobitel = input.KontaktBrojMobitel,
                KontaktEmail = input.KontaktEmail,
                ObjekatStatus = true,
                KlijentId = input.KlijentId,
                MjestoId = input.MjestoId
            };

            _context.Objekti.Add(noviObjekat);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(ObjekatUrediVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Detalji", input);
            }

            var stariObjekat = _context.Objekti.Find(input.ObjekatId);

            if (stariObjekat != null)
            {
                stariObjekat.Ulica = input.Ulica;
                stariObjekat.KontaktOsoba = input.KontaktOsoba;
                stariObjekat.KontaktBrojFiksni = input.KontaktBrojFiksni;
                stariObjekat.KontaktBrojMobitel = input.KontaktBrojMobitel;
                stariObjekat.KontaktEmail = input.KontaktEmail;
                stariObjekat.ObjekatStatus = input.ObjekatStatus;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var objekat = _context.Objekti.Find(id);

            if (objekat == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Objekti.Remove(objekat);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}