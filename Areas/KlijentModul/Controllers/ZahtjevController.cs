using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.KlijentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.KlijentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: false, client: true)]
    [Area("KlijentModul")]
    public class ZahtjevController : Controller
    {
        private readonly MojContext _context;
        public ZahtjevController(MojContext context)
        {
            _context = context;
        }

        private void GenerisiZahtjevKategoriju(ZahtjevDodajVM model)
        {
            model.ZahtjevKategorija = _context.ZahtjeviKategorije.Select(s => new SelectListItem
            {
                Value = s.ZahtjevKategorijaId.ToString(),
                Text = s.Naziv
            }).ToList();
        }
        public IActionResult Dodaj()
        {
            var model = new ZahtjevDodajVM();

            GenerisiZahtjevKategoriju(model);

            return View(model);
        }

        //snimanje novog zahtjeva
        public IActionResult Snimi(ZahtjevDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiZahtjevKategoriju(input);
                return View("Dodaj", input);
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            var noviZahtjev = new Zahtjev()
            {
                Naslov = input.Naslov,
                Opis = input.Opis,
                DatumKreiranja = DateTime.Now,
                KlijentskiRacunId = korisnik.KlijentskiRacunId,
                ZahtjevKategorijaId = input.ZahtjevKategorijaId,
                ZahtjevStatusId = 1 //1- OTVOREN ZAHTJEV
            };

            _context.Zahtjevi.Add(noviZahtjev);

            _context.SaveChanges();

            return RedirectToAction("Index", "Podrska", new { @area = "" });
        }
    }
}