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
    public class ZahtjevKategorijaController : Controller
    {
        private readonly MojContext _context;
        public ZahtjevKategorijaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraNazivaKategorijeZahtjeva(string naziv, int zahtjevKategorijaId = 0)
        {
            if (zahtjevKategorijaId == 0)
            {
                if (_context.ZahtjeviKategorije.Any(a => a.Naziv.ToUpper() == naziv.ToUpper()))
                {
                    return Json($"Kategorija zahtjeva \"{naziv}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.ZahtjeviKategorije.Any(a => a.Naziv.ToUpper() == naziv.ToUpper() && a.ZahtjevKategorijaId != zahtjevKategorijaId))
                {
                    return Json($"Kategorija zahtjeva \"{naziv}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            var model = new ZahtjevKategorijaIndexVM
            {
                Rows = _context.ZahtjeviKategorije.Select(s => new ZahtjevKategorijaIndexVM.Row()
                {
                    ZahtjevKategorijaId = s.ZahtjevKategorijaId,
                    Naziv = s.Naziv,
                    Opis = s.Opis,
                    DeleteBtn = ((s.Zahtjevi.Where(w => w.ZahtjevKategorijaId == s.ZahtjevKategorijaId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new ZahtjevKategorijaDodajVM();

            return View(model);
        }

        public IActionResult Uredi(int id)
        {
            var zahtjevKategorija = _context.ZahtjeviKategorije.Find(id);

            if (zahtjevKategorija == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ZahtjevKategorijaDodajVM()
            {
                ZahtjevKategorijaId = zahtjevKategorija.ZahtjevKategorijaId,
                Naziv = zahtjevKategorija.Naziv,
                Opis = zahtjevKategorija.Opis
            };

            return View(model);
        }

        public IActionResult Snimi(ZahtjevKategorijaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            ZahtjevKategorija noviZahtjevKategorija = new ZahtjevKategorija()
            {
                Naziv = input.Naziv,
                Opis = input.Opis
            };

            _context.ZahtjeviKategorije.Add(noviZahtjevKategorija);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(ZahtjevKategorijaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
            }

            var stariZahtjevKategorija = _context.ZahtjeviKategorije.Find(input.ZahtjevKategorijaId);

            if (stariZahtjevKategorija != null)
            {
                stariZahtjevKategorija.Naziv = input.Naziv;
                stariZahtjevKategorija.Opis = input.Opis;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var zahtjevKategorija = _context.ZahtjeviKategorije.Find(id);

            if (zahtjevKategorija == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.ZahtjeviKategorije.Remove(zahtjevKategorija);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}