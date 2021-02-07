using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.InzinjerModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.InzinjerModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: true, mgmt: false, client: false)]
    [Area("InzinjerModul")]
    public class ClanServisaController : Controller
    {
        private readonly MojContext _context;
        public ClanServisaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new ClanServisaIndexVM
            {
                Rows = _context.ClanoviServisa.Select(s => new ClanServisaIndexVM.Row
                {
                    ClanServisaId = s.ClanServisaId,
                    ImePrezime = s.Ime + " " + s.Prezime,
                    BrojMobitela = s.BrojMobitela,
                    Zanimanje = s.Zanimanje,
                    ClanServisaStatus = s.ClanServisaStatus,
                    DeleteBtn = ((s.ObavljeniPoslovi.Where(w => w.ClanServisaId == s.ClanServisaId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new ClanServisaDodajVM();

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var clanServisa = _context.ClanoviServisa.Find(id);

            if (clanServisa == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ClanServisaDetaljiVM()
            {
                ClanServisaId = clanServisa.ClanServisaId,
                Ime = clanServisa.Ime,
                Prezime = clanServisa.Prezime,
                BrojMobitela = clanServisa.BrojMobitela,
                Zanimanje = clanServisa.Zanimanje,
                ClanServisaStatus = clanServisa.ClanServisaStatus
            };

            return View(model);
        }

        public IActionResult Snimi(ClanServisaDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            ClanServisa noviClanServisa = new ClanServisa()
            {
                Ime = input.Ime,
                Prezime = input.Prezime,
                BrojMobitela = input.BrojMobitela,
                Zanimanje = input.Zanimanje,
                ClanServisaStatus = true
            };

            _context.ClanoviServisa.Add(noviClanServisa);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(ClanServisaDetaljiVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Detalji", input);
            }

            var stariClanServisa = _context.ClanoviServisa.Find(input.ClanServisaId);

            if (stariClanServisa != null)
            {
                stariClanServisa.BrojMobitela = input.BrojMobitela;
                stariClanServisa.Zanimanje = input.Zanimanje;
                stariClanServisa.ClanServisaStatus = input.ClanServisaStatus;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var clanServisa = _context.ClanoviServisa.Find(id);

            if (clanServisa == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.ClanoviServisa.Remove(clanServisa);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}