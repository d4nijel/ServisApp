using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: true, client: true)]
    public class KreirajIzvjestajController : Controller
    {
        private readonly MojContext _context;
        public IJsReportMVCService JsReportMVCService { get; }
        public KreirajIzvjestajController(MojContext context, IJsReportMVCService jsReportMVCService)
        {
            _context = context;
            JsReportMVCService = jsReportMVCService;
        }

        private void GenerisiNaziveIspitivanja(KreirajIzvjestajIndexVM model)
        {
            model.NaziviIspitivanja = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaStatus == true).Select(s => new SelectListItem
            {
                Value = s.NazivIspitivanjaId.ToString(),
                Text = s.Naziv
            }).ToList();
        }

        private void GenerisiKlijente(KreirajIzvjestajIndexVM model)
        {
            model.Klijenti = _context.Klijenti.Where(w => w.KlijentStatus == true).Select(s => new SelectListItem
            {
                Value = s.KlijentId.ToString(),
                Text = s.SkraceniNaziv + " - " + s.IdBroj
            }).ToList();
        }

        private void GenerisiKantone(KreirajIzvjestajIndexVM model)
        {
            model.Kantoni = _context.Kantoni.Select(s => new SelectListItem
            {
                Value = s.KantonId.ToString(),
                Text = s.SkraceniNaziv
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

        private void GenerisiPodatke(PregledIspitivanjaIndexVM model, int klijentId = 0, int nazivIspitivanjaId = 0, int kantonId = 0, int opcinaId = 0, bool kasnjenje = false)
        {
            List<Objekat> objekti = new List<Objekat>();

            if (klijentId == 0) //svi klijenti
            {
                objekti = _context.Objekti.Where(w => w.ObjekatStatus == true)
                    .Include(i => i.RadniNalozi)
                        .ThenInclude(e => e.Ispitivanja)
                    .Include(n => n.Klijent)
                    .Include(c => c.Mjesto)
                        .ThenInclude(t => t.Opcina)
                            .ThenInclude(h => h.Kanton).ToList();
            }
            else //odredjeni klijent
            {
                objekti = _context.Objekti.Where(w => w.ObjekatStatus == true && w.KlijentId == klijentId)
                    .Include(i => i.RadniNalozi)
                        .ThenInclude(e => e.Ispitivanja)
                    .Include(n => n.Klijent)
                    .Include(c => c.Mjesto)
                        .ThenInclude(t => t.Opcina)
                            .ThenInclude(h => h.Kanton).ToList();
            }

            if (nazivIspitivanjaId == 0) //sve usluge
            {
                model.Usluge = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaStatus == true).OrderBy(o => o.NazivIspitivanjaId).ToDictionary(x => x.NazivIspitivanjaId, x => x.Oznaka);
            }
            else //odredjena usluga
            {
                model.Usluge = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaStatus == true && w.NazivIspitivanjaId == nazivIspitivanjaId).OrderBy(o => o.NazivIspitivanjaId).ToDictionary(x => x.NazivIspitivanjaId, x => x.Oznaka);
            }

            List<int> listaBrojaDanaDoIstekaUsluge = new List<int>();

            model.Rows = new List<PregledIspitivanjaIndexVM.Row>();

            bool kreirajIzvjestaj = false;
            bool kreirajIzvjestajKasnjenja = false;
            int brojDanaDoIstekaUsluge;

            foreach (var objekat in objekti)
            {
                if (kasnjenje)
                {
                    foreach (var item in model.Usluge)
                    {
                        //pronadji datum narednog redovnog ispitivanja za taj objekat za odredjenu uslugu
                        DateTime x = _context.Ispitivanja.Where(w => w.RadniNalog.ObjekatId == objekat.ObjekatId && w.NazivIspitivanjaId == item.Key && w.TipIspitivanja == "Redovno").OrderByDescending(o => o.DatumIspitivanja).Select(s => s.DatumNarednogIspitivanja).FirstOrDefault();
                        if (x != DateTime.MinValue)
                        {
                            brojDanaDoIstekaUsluge = x.Date.Subtract(DateTime.Now.Date).Days;
                            if (brojDanaDoIstekaUsluge <= 20)
                            {
                                kreirajIzvjestajKasnjenja = true;
                            }
                            listaBrojaDanaDoIstekaUsluge.Add(brojDanaDoIstekaUsluge);
                        }
                        else
                        {
                            listaBrojaDanaDoIstekaUsluge.Add(-365);
                        }
                    }

                    if (kreirajIzvjestajKasnjenja)
                    {
                        if (kantonId == 0)
                        {
                            if (opcinaId == 0)
                            {
                                model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                {
                                    ObjekatId = objekat.ObjekatId,
                                    Klijent = objekat.Klijent.SkraceniNaziv,
                                    Objekat = objekat.Naziv,
                                    Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                    Opcina = objekat.Mjesto.Opcina.Naziv,
                                    Mjesto = objekat.Mjesto.Naziv,
                                    BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                });
                            }
                            else
                            {
                                if (objekat.Mjesto.OpcinaId == opcinaId)
                                {
                                    model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                    {
                                        ObjekatId = objekat.ObjekatId,
                                        Klijent = objekat.Klijent.SkraceniNaziv,
                                        Objekat = objekat.Naziv,
                                        Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                        Opcina = objekat.Mjesto.Opcina.Naziv,
                                        Mjesto = objekat.Mjesto.Naziv,
                                        BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                    });
                                }
                                else
                                {
                                    listaBrojaDanaDoIstekaUsluge.Clear();
                                    kreirajIzvjestajKasnjenja = false;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (opcinaId == 0)
                            {
                                if (objekat.Mjesto.Opcina.KantonId == kantonId)
                                {
                                    model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                    {
                                        ObjekatId = objekat.ObjekatId,
                                        Klijent = objekat.Klijent.SkraceniNaziv,
                                        Objekat = objekat.Naziv,
                                        Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                        Opcina = objekat.Mjesto.Opcina.Naziv,
                                        Mjesto = objekat.Mjesto.Naziv,
                                        BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                    });
                                }
                                else
                                {
                                    listaBrojaDanaDoIstekaUsluge.Clear();
                                    kreirajIzvjestajKasnjenja = false;
                                    continue;
                                }
                            }
                            else
                            {
                                if (objekat.Mjesto.OpcinaId == opcinaId && objekat.Mjesto.Opcina.KantonId == kantonId)
                                {
                                    model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                    {
                                        ObjekatId = objekat.ObjekatId,
                                        Klijent = objekat.Klijent.SkraceniNaziv,
                                        Objekat = objekat.Naziv,
                                        Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                        Opcina = objekat.Mjesto.Opcina.Naziv,
                                        Mjesto = objekat.Mjesto.Naziv,
                                        BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                    });
                                }
                                else
                                {
                                    listaBrojaDanaDoIstekaUsluge.Clear();
                                    kreirajIzvjestajKasnjenja = false;
                                    continue;
                                }
                            }
                        }
                    }
                    listaBrojaDanaDoIstekaUsluge.Clear();
                    kreirajIzvjestajKasnjenja = false;
                }
                else
                {
                    foreach (var radniNalog in objekat.RadniNalozi)
                    {
                        if (radniNalog.Ispitivanja.Count > 0)
                        {
                            kreirajIzvjestaj = true;
                            break;
                        }
                    }

                    foreach (var item in model.Usluge)
                    {
                        //pronadji datum narednog redovnog ispitivanja za taj objekat za odredjenu uslugu
                        DateTime x = _context.Ispitivanja.Where(w => w.RadniNalog.ObjekatId == objekat.ObjekatId && w.NazivIspitivanjaId == item.Key && w.TipIspitivanja == "Redovno").OrderByDescending(o => o.DatumIspitivanja).Select(s => s.DatumNarednogIspitivanja).FirstOrDefault();
                        if (x != DateTime.MinValue)
                        {
                            brojDanaDoIstekaUsluge = x.Date.Subtract(DateTime.Now.Date).Days;
                            listaBrojaDanaDoIstekaUsluge.Add(brojDanaDoIstekaUsluge);
                        }
                        else
                        {
                            listaBrojaDanaDoIstekaUsluge.Add(-365);
                        }
                    }

                    if (kreirajIzvjestaj)
                    {
                        if (kantonId == 0)
                        {
                            if (opcinaId == 0)
                            {
                                model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                {
                                    ObjekatId = objekat.ObjekatId,
                                    Klijent = objekat.Klijent.SkraceniNaziv,
                                    Objekat = objekat.Naziv,
                                    Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                    Opcina = objekat.Mjesto.Opcina.Naziv,
                                    Mjesto = objekat.Mjesto.Naziv,
                                    BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                });
                            }
                            else
                            {
                                if (objekat.Mjesto.OpcinaId == opcinaId)
                                {
                                    model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                    {
                                        ObjekatId = objekat.ObjekatId,
                                        Klijent = objekat.Klijent.SkraceniNaziv,
                                        Objekat = objekat.Naziv,
                                        Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                        Opcina = objekat.Mjesto.Opcina.Naziv,
                                        Mjesto = objekat.Mjesto.Naziv,
                                        BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                    });
                                }
                                else
                                {
                                    listaBrojaDanaDoIstekaUsluge.Clear();
                                    kreirajIzvjestaj = false;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (opcinaId == 0)
                            {
                                if (objekat.Mjesto.Opcina.KantonId == kantonId)
                                {
                                    model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                    {
                                        ObjekatId = objekat.ObjekatId,
                                        Klijent = objekat.Klijent.SkraceniNaziv,
                                        Objekat = objekat.Naziv,
                                        Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                        Opcina = objekat.Mjesto.Opcina.Naziv,
                                        Mjesto = objekat.Mjesto.Naziv,
                                        BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                    });
                                }
                                else
                                {
                                    listaBrojaDanaDoIstekaUsluge.Clear();
                                    kreirajIzvjestaj = false;
                                    continue;
                                }
                            }
                            else
                            {
                                if (objekat.Mjesto.OpcinaId == opcinaId && objekat.Mjesto.Opcina.KantonId == kantonId)
                                {
                                    model.Rows.Add(new PregledIspitivanjaIndexVM.Row()
                                    {
                                        ObjekatId = objekat.ObjekatId,
                                        Klijent = objekat.Klijent.SkraceniNaziv,
                                        Objekat = objekat.Naziv,
                                        Kanton = objekat.Mjesto.Opcina.Kanton.SkraceniNaziv,
                                        Opcina = objekat.Mjesto.Opcina.Naziv,
                                        Mjesto = objekat.Mjesto.Naziv,
                                        BrojDanaDoIstekaUsluge = new List<int>(listaBrojaDanaDoIstekaUsluge)
                                    });
                                }
                                else
                                {
                                    listaBrojaDanaDoIstekaUsluge.Clear();
                                    kreirajIzvjestaj = false;
                                    continue;
                                }
                            }
                        }
                    }
                    listaBrojaDanaDoIstekaUsluge.Clear();
                    kreirajIzvjestaj = false;
                }
            }
        }

        public IActionResult OpciIzvjestaj()
        {
            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            var model = new KreirajIzvjestajIndexVM();

            if (korisnik.IsKlijent)
            {
                var klijent = _context.Klijenti.Where(w => w.KlijentskiRacun.KlijentskiRacunId == korisnik.KlijentskiRacunId).SingleOrDefault();

                if (klijent != null)
                {
                    model.KlijentId = klijent.KlijentId;
                    model.KlijentNaziv = klijent.Naziv;
                }

                model.IsKlijent = true;

                GenerisiNaziveIspitivanja(model);
                GenerisiKantone(model);

                return View(model);
            }
            else
            {
                GenerisiNaziveIspitivanja(model);
                GenerisiKlijente(model);
                GenerisiKantone(model);

                return View(model);
            }
        }

        public IActionResult IzvjestajKasnjenja()
        {
            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            var model = new KreirajIzvjestajIndexVM();

            if (korisnik.IsKlijent)
            {
                var klijent = _context.Klijenti.Where(w => w.KlijentskiRacun.KlijentskiRacunId == korisnik.KlijentskiRacunId).SingleOrDefault();

                if (klijent != null)
                {
                    model.KlijentId = klijent.KlijentId;
                    model.KlijentNaziv = klijent.Naziv;
                }

                model.IsKlijent = true;

                GenerisiNaziveIspitivanja(model);
                GenerisiKantone(model);

                return View(model);
            }
            else
            {
                GenerisiNaziveIspitivanja(model);
                GenerisiKlijente(model);
                GenerisiKantone(model);

                return View(model);
            }
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> KreirajOpciIzvjestaj(KreirajIzvjestajIndexVM input)
        {
            var model = new PregledIspitivanjaIndexVM();

            var klijent = _context.Klijenti.Find(input.KlijentId);
            var usluga = _context.NaziviIspitivanja.Find(input.NazivIspitivanjaId);
            var kanton = _context.Kantoni.Find(input.KantonId);
            var opcina = _context.Opcine.Find(input.OpcinaId);

            if (klijent == null && usluga == null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, 0, 0, 0, 0, false);
                model.IsKlijent = false;
                model.IsUsluga = false;
            }

            else if (klijent == null && usluga == null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, 0, 0, kanton.KantonId, 0, false);
                model.IsKlijent = false;
                model.IsUsluga = false;
            }

            else if (klijent == null && usluga == null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, 0, 0, kanton.KantonId, opcina.OpcinaId, false);
                model.IsKlijent = false;
                model.IsUsluga = false;
            }

            else if (klijent == null && usluga != null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, 0, usluga.NazivIspitivanjaId, 0, 0, false);
                model.IsKlijent = false;
                model.IsUsluga = true;
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent == null && usluga != null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, 0, usluga.NazivIspitivanjaId, kanton.KantonId, 0, false);
                model.IsKlijent = false;
                model.IsUsluga = true;
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent == null && usluga != null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, 0, usluga.NazivIspitivanjaId, kanton.KantonId, opcina.OpcinaId, false);
                model.IsKlijent = false;
                model.IsUsluga = true;
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga == null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, 0, 0, 0, false);
                model.IsKlijent = true;
                model.IsUsluga = false;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga == null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, 0, kanton.KantonId, 0, false);
                model.IsKlijent = true;
                model.IsUsluga = false;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga == null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, klijent.KlijentId, 0, kanton.KantonId, opcina.OpcinaId, false);
                model.IsKlijent = true;
                model.IsUsluga = false;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga != null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, usluga.NazivIspitivanjaId, 0, 0, false);
                model.IsKlijent = true;
                model.IsUsluga = true;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga != null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, usluga.NazivIspitivanjaId, kanton.KantonId, 0, false);
                model.IsKlijent = true;
                model.IsUsluga = true;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga != null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, klijent.KlijentId, usluga.NazivIspitivanjaId, kanton.KantonId, opcina.OpcinaId, false);
                model.IsKlijent = true;
                model.IsUsluga = true;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            var header = await JsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "Header", new { });
            var footer = await JsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "Footer", new { });

            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf).Configure((r) => r.Template.Chrome = new Chrome
            {
                HeaderTemplate = header,
                FooterTemplate = footer,
                DisplayHeaderFooter = true,
                Landscape = true,
                Format = "A4",
                MarginTop = "1cm",
                MarginLeft = "0.5cm",
                MarginBottom = "1cm",
                MarginRight = "0.5cm"
            });

            return View("OpciIzvjestajPrint", model);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> KreirajIzvjestajKasnjenja(KreirajIzvjestajIndexVM input)
        {
            var model = new PregledIspitivanjaIndexVM();

            var klijent = _context.Klijenti.Find(input.KlijentId);
            var usluga = _context.NaziviIspitivanja.Find(input.NazivIspitivanjaId);
            var kanton = _context.Kantoni.Find(input.KantonId);
            var opcina = _context.Opcine.Find(input.OpcinaId);

            if (klijent == null && usluga == null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, 0, 0, 0, 0, true);
                model.IsKlijent = false;
                model.IsUsluga = false;
            }

            else if (klijent == null && usluga == null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, 0, 0, kanton.KantonId, 0, true);
                model.IsKlijent = false;
                model.IsUsluga = false;
            }

            else if (klijent == null && usluga == null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, 0, 0, kanton.KantonId, opcina.OpcinaId, true);
                model.IsKlijent = false;
                model.IsUsluga = false;
            }

            else if (klijent == null && usluga != null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, 0, usluga.NazivIspitivanjaId, 0, 0, true);
                model.IsKlijent = false;
                model.IsUsluga = true;
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent == null && usluga != null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, 0, usluga.NazivIspitivanjaId, kanton.KantonId, 0, true);
                model.IsKlijent = false;
                model.IsUsluga = true;
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent == null && usluga != null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, 0, usluga.NazivIspitivanjaId, kanton.KantonId, opcina.OpcinaId, true);
                model.IsKlijent = false;
                model.IsUsluga = true;
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga == null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, 0, 0, 0, true);
                model.IsKlijent = true;
                model.IsUsluga = false;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga == null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, 0, kanton.KantonId, 0, true);
                model.IsKlijent = true;
                model.IsUsluga = false;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga == null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, klijent.KlijentId, 0, kanton.KantonId, opcina.OpcinaId, true);
                model.IsKlijent = true;
                model.IsUsluga = false;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga != null && kanton == null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, usluga.NazivIspitivanjaId, 0, 0, true);
                model.IsKlijent = true;
                model.IsUsluga = true;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga != null && kanton != null && opcina == null)
            {
                GenerisiPodatke(model, klijent.KlijentId, usluga.NazivIspitivanjaId, kanton.KantonId, 0, true);
                model.IsKlijent = true;
                model.IsUsluga = true;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            else if (klijent != null && usluga != null && kanton != null && opcina != null)
            {
                GenerisiPodatke(model, klijent.KlijentId, usluga.NazivIspitivanjaId, kanton.KantonId, opcina.OpcinaId, true);
                model.IsKlijent = true;
                model.IsUsluga = true;
                model.Klijent = _context.Klijenti.Where(w => w.KlijentId == klijent.KlijentId).Select(s => s.Naziv).SingleOrDefault();
                model.Usluga = _context.NaziviIspitivanja.Where(w => w.NazivIspitivanjaId == usluga.NazivIspitivanjaId).Select(s => s.Naziv).SingleOrDefault();
            }

            var header = await JsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "Header", new { });
            var footer = await JsReportMVCService.RenderViewToStringAsync(HttpContext, RouteData, "Footer", new { });

            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf).Configure((r) => r.Template.Chrome = new Chrome
            {
                HeaderTemplate = header,
                FooterTemplate = footer,
                DisplayHeaderFooter = true,
                Landscape = true,
                Format = "A4",
                MarginTop = "1cm",
                MarginLeft = "0.5cm",
                MarginBottom = "1cm",
                MarginRight = "0.5cm"
            });

            return View("IzvjestajKasnjenjaPrint", model);
        }
    }
}