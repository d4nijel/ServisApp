using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.AdministratorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.AdministratorModul.Controllers
{
    [Autorizacija(admin: true, org: false, ing: false, mgmt: false, client: false)]
    [Area("AdministratorModul")]
    public class MjestoController : Controller
    {
        private readonly MojContext _context;
        public MjestoController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraNazivaMjesta(string naziv, int kantonId, int opcinaId, int mjestoId)
        {
            if (mjestoId == 0)
            {
                if (_context.Mjesta.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.OpcinaId == opcinaId && a.Opcina.KantonId == kantonId))
                {
                    var opcina = _context.Opcine.Find(opcinaId);
                    var kanton = _context.Kantoni.Find(kantonId);
                    return Json($"Mjesto \"{naziv}\" u općini \"{opcina.Naziv}\" i kantonu \"{kanton.SkraceniNaziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Mjesta.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.OpcinaId == opcinaId && a.Opcina.KantonId == kantonId && a.MjestoId != mjestoId))
                {
                    var opcina = _context.Opcine.Find(opcinaId);
                    var kanton = _context.Kantoni.Find(kantonId);
                    return Json($"Mjesto \"{naziv}\" u općini \"{opcina.Naziv}\" i kantonu \"{kanton.SkraceniNaziv}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        private void GenerisiKantone(MjestoDodajVM model)
        {
            model.Kantoni = _context.Kantoni.Select(s => new SelectListItem
            {
                Value = s.KantonId.ToString(),
                Text = s.SkraceniNaziv
            }).ToList();
        }

        private void GenerisiOpcineZaKanton(MjestoDodajVM model)
        {
            model.Opcine = _context.Opcine.Where(w => w.KantonId == model.KantonId).Select(s => new SelectListItem
            {
                Value = s.OpcinaId.ToString(),
                Text = s.Naziv
            }).ToList();
        }

        public JsonResult GetListaOpcina(int kantonId)
        {
            List<SelectListItem> Opcine = new List<SelectListItem>();

            Opcine = _context.Opcine.Where(w => w.KantonId == kantonId).Select(s => new SelectListItem
            {
                Value = s.OpcinaId.ToString(),
                Text = s.Naziv
            }).ToList();

            return Json(Opcine);
        }

        public IActionResult Index()
        {
            var model = new MjestoIndexVM
            {
                Rows = _context.Mjesta.Select(s => new MjestoIndexVM.Row()
                {
                    MjestoId = s.MjestoId,
                    Naziv = s.Naziv,
                    Opcina = s.Opcina.Naziv,
                    Kanton = s.Opcina.Kanton.SkraceniNaziv,
                    DeleteBtn = ((s.Klijenti.Where(w => w.MjestoId == s.MjestoId).SingleOrDefault()) == null &&
                    (s.Objekti.Where(w => w.MjestoId == s.MjestoId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new MjestoDodajVM();

            GenerisiKantone(model);

            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var mjesto = _context.Mjesta.Where(w => w.MjestoId == id).Include(i => i.Opcina).SingleOrDefault();

            if (mjesto == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new MjestoDodajVM()
            {
                MjestoId = mjesto.MjestoId,
                Naziv = mjesto.Naziv,
                OpcinaId = mjesto.OpcinaId,
                KantonId = mjesto.Opcina.KantonId
            };

            GenerisiKantone(model);

            GenerisiOpcineZaKanton(model);

            return View(model);
        }

        public IActionResult Snimi(MjestoDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiKantone(input);
                return View("Dodaj", input);
            }

            Mjesto novoMjesto = new Mjesto()
            {
                Naziv = input.Naziv,
                OpcinaId = input.OpcinaId
            };

            _context.Mjesta.Add(novoMjesto);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(MjestoDodajVM input)
        {
            if (!ModelState.IsValid)
                {
                    GenerisiKantone(input);
                    GenerisiOpcineZaKanton(input);

                    return View("Uredi", input);
                }

            var staroMjesto = _context.Mjesta.Find(input.MjestoId);

            if (staroMjesto != null)
            {
                staroMjesto.Naziv = input.Naziv;
                staroMjesto.OpcinaId = input.OpcinaId;

                _context.SaveChanges();
            }
                      
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var mjesto = _context.Mjesta.Find(id);

            if (mjesto == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Mjesta.Remove(mjesto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}