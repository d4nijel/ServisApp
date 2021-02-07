using System;
using System.Collections.Generic;
using System.Linq;
using jsreport.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Data;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: true, client: true)]
    public class PregledIspitivanjaController : Controller
    {
        private readonly MojContext _context;
        public IJsReportMVCService JsReportMVCService { get; }
        public PregledIspitivanjaController(MojContext context, IJsReportMVCService jsReportMVCService)
        {
            _context = context;
            JsReportMVCService = jsReportMVCService;
        }

        public IActionResult Index()
        {
            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            var model = new PregledIspitivanjaIndexVM();

            if (korisnik.IsKlijent)
            {
                var klijentskiRacun = _context.KlijentskiRacuni.Find(korisnik.KlijentskiRacunId);

                if (klijentskiRacun != null)
                {
                    model.Rows = _context.Objekti.Where(w => w.ObjekatStatus == true && w.KlijentId == klijentskiRacun.KlijentId && w.RadniNalozi.Count > 0).Select(s => new PregledIspitivanjaIndexVM.Row
                    {
                        ObjekatId = s.ObjekatId,
                        Klijent = s.Klijent.SkraceniNaziv,
                        Objekat = s.Naziv,
                        Kanton = s.Mjesto.Opcina.Kanton.SkraceniNaziv,
                        Opcina = s.Mjesto.Opcina.Naziv,
                        Mjesto = s.Mjesto.Naziv,
                    }).ToList();
                }
            }
            else
            {
                model.Rows = _context.Objekti.Where(w => w.ObjekatStatus == true && w.RadniNalozi.Count > 0).Select(s => new PregledIspitivanjaIndexVM.Row
                {
                    ObjekatId = s.ObjekatId,
                    Klijent = s.Klijent.SkraceniNaziv,
                    Objekat = s.Naziv,
                    Kanton = s.Mjesto.Opcina.Kanton.SkraceniNaziv,
                    Opcina = s.Mjesto.Opcina.Naziv,
                    Mjesto = s.Mjesto.Naziv,
                }).ToList();
            }

            model.Usluge = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaStatus == true).OrderBy(o => o.NazivIspitivanjaId).ToDictionary(x => x.NazivIspitivanjaId, x => x.Oznaka);

            foreach (var item in model.Rows)
            {
                item.BrojDanaDoIstekaUsluge = new List<int>();
                foreach (var item2 in model.Usluge)
                {
                    DateTime x = _context.Ispitivanja.Where(w => w.RadniNalog.ObjekatId == item.ObjekatId && w.NazivIspitivanjaId == item2.Key && w.TipIspitivanja == "Redovno").OrderByDescending(o => o.DatumIspitivanja).Select(s => s.DatumNarednogIspitivanja).FirstOrDefault();
                    if (x != DateTime.MinValue)
                    {
                        item.BrojDanaDoIstekaUsluge.Add(x.Date.Subtract(DateTime.Now.Date).Days);
                    }
                    else
                    {
                        item.BrojDanaDoIstekaUsluge.Add(-365);
                    }
                }
            }

            return View(model);
        }
    }
}