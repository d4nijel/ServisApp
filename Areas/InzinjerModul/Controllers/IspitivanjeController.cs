using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.InzinjerModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.InzinjerModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: true, mgmt: false, client: false)]
    [Area("InzinjerModul")]
    public class IspitivanjeController : Controller
    {
        private readonly MojContext _context;
        public IspitivanjeController(MojContext context)
        {
            _context = context;
        }

        private void GenerisiNaziveIspitivanja(IspitivanjeDodajVM model)
        {
            var sviNaziviIspitivanja = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaStatus == true).ToList();
            var odabraniNaziviIspitivanja = _context.Ispitivanja.Where(w => w.RadniNalogId == model.RadniNalogId).Select(s => s.NazivIspitivanjaId).ToList();
            model.NaziviIspitivanja = new List<SelectListItem>();

            bool t = false;
            foreach (var item in sviNaziviIspitivanja)
            {
                foreach (var item2 in odabraniNaziviIspitivanja)
                {
                    if (item.NazivIspitivanjaId == item2)
                    {
                        t = true;
                        break;
                    }
                }
                if (!t)
                {
                    SelectListItem selListItem = new SelectListItem() { Value = item.NazivIspitivanjaId.ToString(), Text = item.Naziv };
                    model.NaziviIspitivanja.Add(selListItem);
                }
                t = false;
            }
        }

        public IActionResult Index(int id)
        {
            var radniNalog = _context.RadniNalozi.Find(id);

            if (radniNalog == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new IspitivanjeIndexVM
            {
                Rows = _context.Ispitivanja.Where(w => w.RadniNalogId == radniNalog.RadniNalogId).Select(s => new IspitivanjeIndexVM.Row
                {
                    IspitivanjeId = s.IspitivanjeId,
                    NazivIspitivanja = s.NazivIspitivanja.Oznaka,
                    DatumIspitivanja = s.DatumIspitivanja.ToString("dd.MM.yyyy"),
                    DatumNarednogIspitivanja = s.DatumNarednogIspitivanja.ToString("dd.MM.yyyy"),
                    BroDanaDoNarednogIspitivanja = s.DatumNarednogIspitivanja.Date.Subtract(DateTime.Now.Date).Days,
                    TipIspitivanja = s.TipIspitivanja,
                    PostojanjeIzvjestaja = (s.Izvjestaj == null) ? false : true,
                    IzvjestajId = (s.Izvjestaj != null) ? s.Izvjestaj.IzvjestajId : 0,
                    DeleteBtn = (s.Izvjestaj == null) ? true : false
                }).ToList(),
                RadniNalogId = radniNalog.RadniNalogId,
                //BrojRadnogNaloga = radniNalog.BrojRadnogNaloga
            };

            return PartialView(model);
        }

        public IActionResult Dodaj(int id)
        {
            var radniNalog = _context.RadniNalozi.Find(id);

            if (radniNalog == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new IspitivanjeDodajVM()
            {
                DatumIspitivanja = radniNalog.DatumZavrsetkaRadova,
                RadniNalogId = radniNalog.RadniNalogId
            };
            GenerisiNaziveIspitivanja(model);

            return PartialView(model);
        }

        public IActionResult Detalji(int id)
        {
            var ispitivanje = _context.Ispitivanja.Where(w => w.IspitivanjeId == id).Include(i => i.NazivIspitivanja).SingleOrDefault();

            if (ispitivanje == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new IspitivanjeDetaljiVM()
            {
                RadniNalogId = ispitivanje.RadniNalogId,
                DatumIspitivanja = ispitivanje.DatumIspitivanja.Date.ToString("dd.MM.yyyy"),
                DatumNarednogIspitivanja = ispitivanje.DatumNarednogIspitivanja.Date.ToString("dd.MM.yyyy"),
                BrojDanaDoNarednogIspitivanja = ispitivanje.DatumNarednogIspitivanja.Date.Subtract(DateTime.Now.Date).Days.ToString(),
                TipIspitivanja = ispitivanje.TipIspitivanja,
                Napomena = ispitivanje.Napomena,
                NazivIspitivanja = ispitivanje.NazivIspitivanja.Naziv,
                NazivIspitivanjaOznaka = ispitivanje.NazivIspitivanja.Oznaka
            };

            return PartialView(model);
        }

        public IActionResult Snimi(IspitivanjeDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiNaziveIspitivanja(input);
                return PartialView("Dodaj", input);
            }

            int? period = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == input.NazivIspitivanjaId).Select(s => s.PeriodVazenja).SingleOrDefault();

            if (period != null)
            {
                Ispitivanje novoIspitivanje = new Ispitivanje
                {
                    DatumIspitivanja = input.DatumIspitivanja.Date,
                    DatumNarednogIspitivanja = input.DatumIspitivanja.Date.AddMonths(period.Value),
                    TipIspitivanja = input.TipIspitivanja,
                    Napomena = input.Napomena,
                    RadniNalogId = input.RadniNalogId,
                    NazivIspitivanjaId = input.NazivIspitivanjaId
                };

                _context.Ispitivanja.Add(novoIspitivanje);
                _context.SaveChanges();
            }

            return Redirect("/InzinjerModul/Ispitivanje/Index?Id=" + input.RadniNalogId);
        }

        public IActionResult Obrisi(int id)
        {
            var ispitivanje = _context.Ispitivanja.Find(id);

            if (ispitivanje == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Ispitivanja.Remove(ispitivanje);
            _context.SaveChanges();

            return Redirect("/InzinjerModul/Ispitivanje/Index?Id=" + ispitivanje.RadniNalogId);
        }
    }
}