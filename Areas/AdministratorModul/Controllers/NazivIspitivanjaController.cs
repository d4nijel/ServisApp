using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.AdministratorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.AdministratorModul.Controllers
{
    [Autorizacija(admin: true, org: false, ing: false, mgmt: false, client: false)]
    [Area("AdministratorModul")]
    public class NazivIspitivanjaController : Controller
    {
        private readonly MojContext _context;
        public NazivIspitivanjaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraOznake(string oznaka, int nazivIspitivanjaId = 0)
        {
            if (nazivIspitivanjaId == 0)
            {
                if (_context.NaziviIspitivanja.Any(a => a.Oznaka.ToUpper() == oznaka.ToUpper()))
                {
                    return Json($"Oznaka \"{oznaka}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.NaziviIspitivanja.Any(a => a.Oznaka.ToUpper() == oznaka.ToUpper() && a.NazivIspitivanjaId != nazivIspitivanjaId))
                {
                    return Json($"Oznaka \"{oznaka}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult ProvjeraNaziva(string naziv, int nazivIspitivanjaId = 0)
        {
            if (nazivIspitivanjaId == 0)
            {
                if (_context.NaziviIspitivanja.Any(a => a.Naziv.ToUpper() == naziv.ToUpper()))
                {
                    return Json($"Naziv usluge \"{naziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.NaziviIspitivanja.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.NazivIspitivanjaId != nazivIspitivanjaId))
                {
                    return Json($"Naziv usluge \"{naziv}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            var model = new NazivIspitivanjaIndexVM
            {
                Rows = _context.NaziviIspitivanja.Select(s => new NazivIspitivanjaIndexVM.Row()
                {
                    NazivIspitivanjaId = s.NazivIspitivanjaId,
                    Naziv = s.Naziv,
                    Oznaka = s.Oznaka,
                    PeriodVazenja = s.PeriodVazenja,
                    NazivIspitivanjaStatus = s.NazivIspitivanjaStatus,
                    DeleteBtn = ((s.Ispitivanja.Where(w => w.NazivIspitivanjaId == s.NazivIspitivanjaId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new NazivIspitivanjaDodajVM()
            {
                PeriodVazenja = 6
            };

            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var nazivIspitivanja = _context.NaziviIspitivanja.Find(id);

            if (nazivIspitivanja == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new NazivIspitivanjaDodajVM()
            {
                NazivIspitivanjaId = nazivIspitivanja.NazivIspitivanjaId,
                Naziv = nazivIspitivanja.Naziv,
                Oznaka = nazivIspitivanja.Oznaka,
                PeriodVazenja = nazivIspitivanja.PeriodVazenja,
                NazivIspitivanjaStatus = nazivIspitivanja.NazivIspitivanjaStatus
            };

            return View(model);
        }

        public IActionResult Snimi(NazivIspitivanjaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            NazivIspitivanja noviNazivIspitivanja = new NazivIspitivanja()
            {
                Naziv = input.Naziv,
                Oznaka = input.Oznaka,
                PeriodVazenja = input.PeriodVazenja,
                NazivIspitivanjaStatus = true
            };

            _context.NaziviIspitivanja.Add(noviNazivIspitivanja);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(NazivIspitivanjaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
            }

            var stariNazivIspitivanja = _context.NaziviIspitivanja.Find(input.NazivIspitivanjaId);

            if (stariNazivIspitivanja != null)
            {
                stariNazivIspitivanja.Naziv = input.Naziv;
                stariNazivIspitivanja.Oznaka = input.Oznaka;
                stariNazivIspitivanja.PeriodVazenja = input.PeriodVazenja;
                stariNazivIspitivanja.NazivIspitivanjaStatus = input.NazivIspitivanjaStatus;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var nazivIspitivanja = _context.NaziviIspitivanja.Find(id);

            if (nazivIspitivanja == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.NaziviIspitivanja.Remove(nazivIspitivanja);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}