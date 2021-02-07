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
    public class ZahtjevStatusController : Controller
    {
        private readonly MojContext _context;
        public ZahtjevStatusController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraNazivaStatusaZahtjeva(string naziv, int zahtjevStatusId = 0)
        {
            if (zahtjevStatusId == 0)
            {
                if (_context.ZahtjeviStatusi.Any(a => a.Naziv.ToUpper() == naziv.ToUpper()))
                {
                    return Json($"Status zahtjeva \"{naziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.ZahtjeviStatusi.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.ZahtjevStatusId != zahtjevStatusId))
                {
                    return Json($"Status zahtjeva \"{naziv}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            var model = new ZahtjevStatusIndexVM
            {
                Rows = _context.ZahtjeviStatusi.Select(s => new ZahtjevStatusIndexVM.Row()
                {
                    ZahtjevStatusId = s.ZahtjevStatusId,
                    Naziv = s.Naziv,
                    Opis = s.Opis,
                    DeleteBtn = ((s.Zahtjevi.Where(w => w.ZahtjevStatusId == s.ZahtjevStatusId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new ZahtjevStatusDodajVM();

            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var zahtjevStatus = _context.ZahtjeviStatusi.Find(id);

            if (zahtjevStatus == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ZahtjevStatusDodajVM()
            {
                ZahtjevStatusId = zahtjevStatus.ZahtjevStatusId,
                Naziv = zahtjevStatus.Naziv,
                Opis = zahtjevStatus.Opis
            };

            return View(model);
        }

        public IActionResult Snimi(ZahtjevStatusDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            ZahtjevStatus noviZahtjevStatus = new ZahtjevStatus()
            {
                Naziv = input.Naziv,
                Opis = input.Opis
            };

            _context.ZahtjeviStatusi.Add(noviZahtjevStatus);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(ZahtjevStatusDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
            }

            var stariZahtjevStatus = _context.ZahtjeviStatusi.Find(input.ZahtjevStatusId);

            if (stariZahtjevStatus != null)
            {
                stariZahtjevStatus.Naziv = input.Naziv;
                stariZahtjevStatus.Opis = input.Opis;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var zahtjevStatus = _context.ZahtjeviStatusi.Find(id);

            if (zahtjevStatus == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.ZahtjeviStatusi.Remove(zahtjevStatus);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}