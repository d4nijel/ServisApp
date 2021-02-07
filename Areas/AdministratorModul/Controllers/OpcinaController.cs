using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.AdministratorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.AdministratorModul.Controllers
{
    [Autorizacija(admin: true, org: false, ing: false, mgmt: false, client: false)]
    [Area("AdministratorModul")]
    public class OpcinaController : Controller
    {
        private readonly MojContext _context;
        public OpcinaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraNazivaOpcine(string naziv, int kantonId, int opcinaId = 0)
        {
            if (opcinaId == 0)
            {
                if (_context.Opcine.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.KantonId == kantonId))
                {
                    var kanton = _context.Kantoni.Find(kantonId);
                    return Json($"Općina \"{naziv}\" u kantonu \"{kanton.SkraceniNaziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Opcine.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.KantonId == kantonId && a.OpcinaId != opcinaId))
                {
                    var kanton = _context.Kantoni.Find(kantonId);
                    return Json($"Općina \"{naziv}\" u kantonu \"{kanton.SkraceniNaziv}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        private void GenerisiKantone(OpcinaDodajVM model)
        {
            model.Kantoni = _context.Kantoni.Select(s => new SelectListItem
            {
                Value = s.KantonId.ToString(),
                Text = s.SkraceniNaziv
            }).ToList();
        }

        public IActionResult Index()
        {
            var model = new OpcinaIndexVM
            {
                Rows = _context.Opcine.Select(s => new OpcinaIndexVM.Row()
                {
                    OpcinaId = s.OpcinaId,
                    Naziv = s.Naziv,
                    Kanton = s.Kanton.SkraceniNaziv,
                    BrojMjesta = s.Mjesta.Where(w => w.OpcinaId == s.OpcinaId).Count(),
                    DeleteBtn = ((s.Mjesta.Where(w => w.OpcinaId == s.OpcinaId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new OpcinaDodajVM();

            GenerisiKantone(model);

            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var opcina = _context.Opcine.Find(id);

            if (opcina == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new OpcinaDodajVM()
            {
                OpcinaId = opcina.OpcinaId,
                Naziv = opcina.Naziv,
                KantonId = opcina.KantonId
            };

            GenerisiKantone(model);

            return View(model);
        }

        public IActionResult Snimi(OpcinaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiKantone(input);
                return View("Dodaj", input);
            }

            Opcina novaOpcina = new Opcina()
            {
                Naziv = input.Naziv,
                KantonId = input.KantonId
            };

            _context.Opcine.Add(novaOpcina);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(OpcinaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiKantone(input);
                return View("Uredi", input);
            }

            var staraOpcina = _context.Opcine.Find(input.OpcinaId);

            if (staraOpcina != null)
            {
                staraOpcina.Naziv = input.Naziv;
                staraOpcina.KantonId = input.KantonId;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var opcina = _context.Opcine.Find(id);

            if (opcina == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Opcine.Remove(opcina);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}