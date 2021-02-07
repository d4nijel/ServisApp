using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using ServisApp.Areas.KlijentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: false, org: true, ing: true, mgmt: false, client: true)]
    public class PodrskaController : Controller
    {
        private readonly MojContext _context;
        public PodrskaController(MojContext context)
        {
            _context = context;
        }
        //prikaz svih otvorenih i u procesu rješavanja
        public IActionResult Index()
        {
            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            var model = new ZahtjevIndexVM();

            if (korisnik.IsKlijent)
            {
                //samo OTVORENI I U PROCESU RJEŠAVANJA
                model.Rows = _context.Zahtjevi.Where(w => w.KlijentskiRacunId == korisnik.KlijentskiRacunId && w.ZahtjevStatusId != 3).Select(s => new ZahtjevIndexVM.Row
                {
                    ZahtjevId = s.ZahtjevId,
                    Naslov = s.Naslov,
                    DatumKreiranja = s.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                    KategorijaZahtjeva = s.ZahtjevKategorija.Naziv,
                    StatusZahtjevaId = s.ZahtjevStatus.ZahtjevStatusId,
                    StatusZahtjeva = s.ZahtjevStatus.Naziv,
                    Korisnik = (s.Korisnik != null) ? s.Korisnik.Ime + " " + s.Korisnik.Prezime : "Nije dodijeljen korisnik",
                    KlijentskiRacun = s.KlijentskiRacun.Ime + " " + s.KlijentskiRacun.Prezime
                }).ToList();
                model.IsKlijent = true;
            }
            else
            {
                //samo U PROCESU RJEŠAVANJA 
                model.Rows = _context.Zahtjevi.Where(w => w.KorisnikId == korisnik.KorisnikId && w.ZahtjevStatusId == 2).Select(s => new ZahtjevIndexVM.Row
                {
                    ZahtjevId = s.ZahtjevId,
                    Naslov = s.Naslov,
                    DatumKreiranja = s.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                    KategorijaZahtjeva = s.ZahtjevKategorija.Naziv,
                    StatusZahtjevaId = s.ZahtjevStatus.ZahtjevStatusId,
                    StatusZahtjeva = s.ZahtjevStatus.Naziv,
                    Korisnik = (s.Korisnik != null) ? s.Korisnik.Ime + " " + s.Korisnik.Prezime : "Nije dodijeljen korisnik",
                    KlijentskiRacun = s.KlijentskiRacun.Ime + " " + s.KlijentskiRacun.Prezime,
                    Klijent = s.KlijentskiRacun.Klijent.SkraceniNaziv
                }).ToList();
            }
            return View(model);
        }

        public IActionResult Detalji(int id, bool arhiva)
        {
            var zahtjev = _context.Zahtjevi.Where(w => w.ZahtjevId == id).Include(i => i.KlijentskiRacun).ThenInclude(t => t.Klijent).Include(n => n.ZahtjevKategorija).Include(c => c.ZahtjevStatus).Include(l => l.Korisnik).SingleOrDefault();
            var poruke = _context.Poruke.Where(w => w.ZahtjevId == id).Include(i => i.KlijentskiRacun).Include(n => n.Korisnici).ToList();

            if (zahtjev == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ZahtjevDetaljiVM()
            {
                ZahtjevId = zahtjev.ZahtjevId,
                Naslov = zahtjev.Naslov,
                Opis = zahtjev.Opis,
                DatumKreiranja = zahtjev.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                KlijentskiRacun = zahtjev.KlijentskiRacun.Ime + " " + zahtjev.KlijentskiRacun.Prezime,
                ZahtjevKategorija = zahtjev.ZahtjevKategorija.Naziv,
                StatusZahtjevaId = zahtjev.ZahtjevStatusId,
                StatusZahtjevaNaziv = zahtjev.ZahtjevStatus.Naziv,
                Korisnik = (zahtjev.Korisnik != null) ? zahtjev.Korisnik.Ime + " " + zahtjev.Korisnik.Prezime : "Nije dodijeljen korisnik",
                Klijent = zahtjev.KlijentskiRacun.Klijent.SkraceniNaziv
            };

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            if (!korisnik.IsKlijent)
            {
                model.IsKorisnik = true;
                if (zahtjev.Korisnik != null)
                {
                    if (korisnik.KorisnikId == zahtjev.KorisnikId && zahtjev.ZahtjevStatusId != 3)
                    {
                        model.Zakljucaj = true;
                    }
                }
            }

            model.ListaPoruka = new List<PorukeDetaljiVM>();

            foreach (var item in poruke)
            {
                PorukeDetaljiVM poruka = new PorukeDetaljiVM()
                {
                    Sadrzaj = item.Sadrzaj,
                    DatumKreiranja = item.DatumKreiranja.ToString("dd.MM.yyyy HH:mm")
                };
                if (item.KlijentskiRacunId != null)
                {
                    poruka.KlijentskiRacunNaziv = item.KlijentskiRacun.Ime + " " + item.KlijentskiRacun.Prezime;
                    var x = ImageHelper.GetImageType(item.KlijentskiRacun.KlijentskiRacunSlika);
                    poruka.KlijentskiRacunSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(item.KlijentskiRacun.KlijentskiRacunSlika));
                    poruka.KlijentskiRacun = true;
                }
                else
                {
                    poruka.KorisnikNaziv = item.Korisnici.Ime + " " + item.Korisnici.Prezime;
                    var x = ImageHelper.GetImageType(item.Korisnici.KorisnikSlika);
                    poruka.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(item.Korisnici.KorisnikSlika));
                }

                model.ListaPoruka.Add(poruka);
            }

            if (arhiva)
            {
                model.IsArhiva = true;
            }
            return View(model);
        }

        public IActionResult Arhiva()
        {
            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            var model = new ZahtjevIndexVM();

            if (korisnik.IsKlijent)
            {
                //samo ZAVRSENI ZAHTJEVI 
                model.Rows = _context.Zahtjevi.Where(w => w.KlijentskiRacunId == korisnik.KlijentskiRacunId && w.ZahtjevStatusId == 3).Select(s => new ZahtjevIndexVM.Row
                {
                    ZahtjevId = s.ZahtjevId,
                    Naslov = s.Naslov,
                    DatumKreiranja = s.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                    KategorijaZahtjeva = s.ZahtjevKategorija.Naziv,
                    StatusZahtjevaId = s.ZahtjevStatus.ZahtjevStatusId,
                    StatusZahtjeva = s.ZahtjevStatus.Naziv,
                    Korisnik = (s.Korisnik != null) ? s.Korisnik.Ime + " " + s.Korisnik.Prezime : "Nije dodijeljen korisnik",
                    KlijentskiRacun = s.KlijentskiRacun.Ime + " " + s.KlijentskiRacun.Prezime
                }).ToList();
                model.IsKlijent = true;
            }
            else
            {
                //samo ZAVRSENI ZAHTJEVI 
                model.Rows = _context.Zahtjevi.Where(w => w.KorisnikId == korisnik.KorisnikId && w.ZahtjevStatusId == 3).Select(s => new ZahtjevIndexVM.Row
                {
                    ZahtjevId = s.ZahtjevId,
                    Naslov = s.Naslov,
                    DatumKreiranja = s.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                    KategorijaZahtjeva = s.ZahtjevKategorija.Naziv,
                    StatusZahtjevaId = s.ZahtjevStatus.ZahtjevStatusId,
                    StatusZahtjeva = s.ZahtjevStatus.Naziv,
                    Korisnik = (s.Korisnik != null) ? s.Korisnik.Ime + " " + s.Korisnik.Prezime : "Nije dodijeljen korisnik",
                    KlijentskiRacun = s.KlijentskiRacun.Ime + " " + s.KlijentskiRacun.Prezime,
                    Klijent = s.KlijentskiRacun.Klijent.SkraceniNaziv
                }).ToList();
            }
            model.IsArhiva = true;

            return View("Index", model);
        }

        public IActionResult SnimiPoruku(int ZahtjevId, string sadrzaj)
        {
            if (!ModelState.IsValid)
            {
                var zahtjev = _context.Zahtjevi.Where(w => w.ZahtjevId == ZahtjevId).Include(i => i.KlijentskiRacun).Include(n => n.ZahtjevKategorija).Include(c => c.ZahtjevStatus).Include(l => l.Korisnik).SingleOrDefault();
                var poruke = _context.Poruke.Where(w => w.ZahtjevId == ZahtjevId).Include(i => i.KlijentskiRacun).Include(n => n.Korisnici).ToList();

                if (zahtjev == null)
                {
                    Response.StatusCode = 404;
                    return View("Views/Shared/Error404.cshtml");
                }

                var model = new ZahtjevDetaljiVM()
                {
                    ZahtjevId = zahtjev.ZahtjevId,
                    Naslov = zahtjev.Naslov,
                    Opis = zahtjev.Opis,
                    DatumKreiranja = zahtjev.DatumKreiranja.ToString("dd.MM.yyyy HH:mm"),
                    KlijentskiRacun = zahtjev.KlijentskiRacun.Ime + " " + zahtjev.KlijentskiRacun.Prezime,
                    ZahtjevKategorija = zahtjev.ZahtjevKategorija.Naziv,
                    StatusZahtjevaId = zahtjev.ZahtjevStatusId,
                    StatusZahtjevaNaziv = zahtjev.ZahtjevStatus.Naziv,
                    Korisnik = (zahtjev.Korisnik != null) ? zahtjev.Korisnik.Ime + " " + zahtjev.Korisnik.Prezime : "Nije dodijeljen korisnik"
                };

                AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

                if (!korisnik.IsKlijent)
                {
                    model.IsKorisnik = true;
                    if (zahtjev.Korisnik != null)
                    {
                        if (korisnik.KorisnikId == zahtjev.KorisnikId && zahtjev.ZahtjevStatusId != 3)
                        {
                            model.Zakljucaj = true;
                        }
                    }
                }

                model.ListaPoruka = new List<PorukeDetaljiVM>();

                foreach (var item in poruke)
                {
                    PorukeDetaljiVM poruka = new PorukeDetaljiVM()
                    {
                        Sadrzaj = item.Sadrzaj,
                        DatumKreiranja = item.DatumKreiranja.ToString("dd.MM.yyyy HH:mm")
                    };
                    if (item.KlijentskiRacunId != null)
                    {
                        poruka.KlijentskiRacunNaziv = item.KlijentskiRacun.Ime + " " + item.KlijentskiRacun.Prezime;
                        var x = ImageHelper.GetImageType(item.KlijentskiRacun.KlijentskiRacunSlika);
                        poruka.KlijentskiRacunSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(item.KlijentskiRacun.KlijentskiRacunSlika));
                        poruka.KlijentskiRacun = true;
                    }
                    else
                    {
                        poruka.KorisnikNaziv = item.Korisnici.Ime + " " + item.Korisnici.Prezime;
                        var x = ImageHelper.GetImageType(item.Korisnici.KorisnikSlika);
                        poruka.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(item.Korisnici.KorisnikSlika));
                    }

                    model.ListaPoruka.Add(poruka);
                }
                model.IsArhiva = false;
                return View("Detalji", model);
            }

            var stariZahtjev = _context.Zahtjevi.Find(ZahtjevId);

            if (stariZahtjev == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            if (!sadrzaj.IsNullOrWhiteSpace())
            {
                AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

                var novaPoruka = new Poruka()
                {
                    Sadrzaj = sadrzaj,
                    DatumKreiranja = DateTime.Now,
                    ZahtjevId = ZahtjevId
                };

                if (korisnik.IsKlijent)
                {
                    novaPoruka.KlijentskiRacunId = korisnik.KlijentskiRacunId;
                }
                else
                {
                    novaPoruka.KorisnikId = korisnik.KorisnikId;
                }

                _context.Poruke.Add(novaPoruka);

                _context.SaveChanges();
            }

            return Redirect("/Podrska/Detalji?Id=" + ZahtjevId);
        }
    }
}