using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.AdministratorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: true, org: false, ing: false, mgmt: false, client: false)]
    [Area("AdministratorModul")]
    public class UlogaController : Controller
    {
        private readonly MojContext _context;
        public UlogaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraNazivaUloge(string naziv, int ulogaId)
        {
            if (ulogaId == 0)
            {
                if (_context.Uloge.Any(a => a.Naziv.ToUpper() == naziv.ToUpper()))
                {
                    return Json($"Uloga \"{naziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.Uloge.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.UlogaId != ulogaId))
                {
                    return Json($"Uloga \"{naziv}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            var model = new UlogaIndexVM
            {
                Rows = _context.Uloge.Select(s => new UlogaIndexVM.Row()
                {
                    UlogaId = s.UlogaId,
                    Naziv = s.Naziv,
                    Opis = s.Opis,
                    DeleteBtn = ((s.Permisije.Where(w => w.UlogaId == s.UlogaId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new UlogaDodajVM();
            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var uloga = _context.Uloge.Find(id);

            if (uloga == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new UlogaDodajVM()
            {
                UlogaId = uloga.UlogaId,
                Naziv = uloga.Naziv,
                Opis = uloga.Opis
            };

            return View(model);
        }

        public IActionResult Snimi(UlogaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            Uloga novaUloga = new Uloga()
            {
                Naziv = input.Naziv,
                Opis = input.Opis
            };

            _context.Uloge.Add(novaUloga);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(UlogaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
            }

            var staraUloga = _context.Uloge.Find(input.UlogaId);

            if (staraUloga != null)
            {
                staraUloga.Naziv = input.Naziv;
                staraUloga.Opis = input.Opis;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var uloga = _context.Uloge.Find(id);

            if (uloga == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Uloge.Remove(uloga);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}