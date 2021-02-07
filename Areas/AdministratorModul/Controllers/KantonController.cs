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
    public class KantonController : Controller
    {
        private readonly MojContext _context;
        public KantonController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraNazivaKantona(string naziv, int kantonId = 0)
        {
            if (kantonId == 0)
            {
                if (_context.Kantoni.Any(a => a.Naziv.ToUpper() == naziv.ToUpper()))
                {
                    return Json($"Naziv \"{naziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Kantoni.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.KantonId != kantonId))
                {
                    return Json($"Naziv \"{naziv}\" se već nalazi u bazi");
                }
            }

            return Json(true);
        }

        public IActionResult ProvjeraSkracenogNazivaKantona(string skraceniNaziv, int kantonId = 0)
        {
            if (kantonId == 0)
            {
                if (_context.Kantoni.Any(a => a.SkraceniNaziv.ToUpper() == skraceniNaziv.ToUpper()))
                {
                    return Json($"Skraćeni naziv \"{skraceniNaziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Kantoni.Any(a => a.SkraceniNaziv.ToUpper() == skraceniNaziv.ToUpper() && a.KantonId != kantonId))
                {
                    return Json($"Skraćeni naziv \"{skraceniNaziv}\" se već nalazi u bazi");
                }
            }

            return Json(true);
        }

        public IActionResult Index()
        {
            var model = new KantonIndexVM
            {
                Rows = _context.Kantoni.Select(s => new KantonIndexVM.Row()
                {
                    KantonId = s.KantonId,
                    Naziv = s.Naziv,
                    SkraceniNaziv = s.SkraceniNaziv,
                    BrojOpcina = s.Opcine.Where(w => w.KantonId == s.KantonId).Count(),
                    DeleteBtn = ((s.Opcine.Where(w => w.KantonId == s.KantonId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new KantonDodajVM();

            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var kanton = _context.Kantoni.Find(id);

            if (kanton == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new KantonDodajVM()
            {
                KantonId = kanton.KantonId,
                Naziv = kanton.Naziv,
                SkraceniNaziv = kanton.SkraceniNaziv
            };

            return View(model);
        }

        public IActionResult Snimi(KantonDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            Kanton noviKanton = new Kanton()
            {
                Naziv = input.Naziv,
                SkraceniNaziv = input.SkraceniNaziv
            };

            _context.Kantoni.Add(noviKanton);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(KantonDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
            }

            var stariKanton = _context.Kantoni.Find(input.KantonId);

            if (stariKanton != null)
            {
                stariKanton.Naziv = input.Naziv;
                stariKanton.SkraceniNaziv = input.SkraceniNaziv;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var kanton = _context.Kantoni.Find(id);

            if (kanton == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Kantoni.Remove(kanton);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}