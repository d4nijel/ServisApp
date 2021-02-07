using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Data;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: true, org: true, ing: true, mgmt: true, client: true)]
    public class HomeController : Controller
    {
        private readonly MojContext _context;
        public HomeController(MojContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            AutentifikacijaVM logiraniKorisnik = HttpContext.GetLogiraniKorisnik();

            HomeIndexVM model = new HomeIndexVM
            {
                Mjesec = DateTime.Now.ToString("MMMM"),
                Godina = DateTime.Now.ToString("yyyy")
            };

            if (logiraniKorisnik.IsKlijent)
            {
                model.IsKlijent = true;

                var klijentskiRacun = _context.KlijentskiRacuni.Where(w => w.KlijentskiRacunId == logiraniKorisnik.KlijentskiRacunId).Include(i => i.Klijent).SingleOrDefault();

                model.BrojObjekata = _context.Objekti.Where(w => w.ObjekatStatus == true && w.KlijentId == klijentskiRacun.KlijentId).Count();
                model.BrojRadnihNaloga = _context.RadniNalozi.Where(w => w.Objekat.KlijentId == klijentskiRacun.KlijentId).Count();
                model.BrojIspitivanja = _context.Ispitivanja.Where(w => w.RadniNalog.Objekat.KlijentId == klijentskiRacun.KlijentId).Count();
                model.BrojIzvještaja = _context.Izvjestaji.Where(w => w.Ispitivanje.RadniNalog.Objekat.KlijentId == klijentskiRacun.KlijentId).Count();

                model.BrojZahtjevaMjesec = _context.Zahtjevi.Where(w => w.KlijentskiRacunId == klijentskiRacun.KlijentskiRacunId && w.DatumKreiranja.Month == DateTime.Now.Month).Count();
                model.BrojRadnihNalogaMjesec = _context.RadniNalozi.Where(w => w.Objekat.KlijentId == klijentskiRacun.KlijentId && w.DatumPocetkaRadova.Month == DateTime.Now.Month).Count();
                model.BrojIspitivanjaMjesec = _context.Ispitivanja.Where(w => w.RadniNalog.Objekat.KlijentId == klijentskiRacun.KlijentId && w.DatumIspitivanja.Month == DateTime.Now.Month).Count();
                model.BrojIzvještajaMjesec = _context.Izvjestaji.Where(w => w.Ispitivanje.RadniNalog.Objekat.KlijentId == klijentskiRacun.KlijentId && w.DatumKreiranja.Month == DateTime.Now.Month).Count();

                model.BrojZahtjeva = _context.Zahtjevi.Where(w => w.KlijentskiRacunId == klijentskiRacun.KlijentskiRacunId && (w.ZahtjevStatusId == 1 || w.ZahtjevStatusId == 2)).Count();
                model.BrojArhiviranihZahtjeva = _context.Zahtjevi.Where(w => w.KlijentskiRacunId == klijentskiRacun.KlijentskiRacunId && w.ZahtjevStatusId == 3).Count();
            }
            else
            {
                model.BrojKlijenata = _context.Klijenti.Where(w => w.KlijentStatus == true).Count();
                model.BrojObjekata = _context.Objekti.Where(w => w.ObjekatStatus == true).Count();
                model.BrojRadnihNaloga = _context.RadniNalozi.Count();
                model.BrojIspitivanja = _context.Ispitivanja.Count();

                model.BrojPonudaMjesec = _context.Ponude.Where(w => w.DatumIzdavanja.Month == DateTime.Now.Month).Count();
                model.BrojUgovoraMjesec = _context.Ugovori.Where(w => w.DatumPotpisivanja.Month == DateTime.Now.Month).Count();
                model.BrojRadnihNalogaMjesec = _context.RadniNalozi.Where(w => w.DatumPocetkaRadova.Month == DateTime.Now.Month).Count();
                model.BrojIspitivanjaMjesec = _context.Ispitivanja.Where(w => w.DatumIspitivanja.Month == DateTime.Now.Month).Count();

                if (logiraniKorisnik.IsOrganizator)
                {
                    model.IsOrganizator = true;

                    model.BrojZahtjevaZaDodijeliti = _context.Zahtjevi.Where(w => w.ZahtjevStatusId == 1).Count();
                    model.BrojZahtjeva = _context.Zahtjevi.Where(w => w.KorisnikId == logiraniKorisnik.KorisnikId && w.ZahtjevStatusId == 2).Count();
                    model.BrojArhiviranihZahtjeva = _context.Zahtjevi.Where(w => w.KorisnikId == logiraniKorisnik.KorisnikId && w.ZahtjevStatusId == 3).Count();
                }

                else if (logiraniKorisnik.IsInzinjer)
                {
                    model.IsInzinjer = true;

                    model.BrojZahtjeva = _context.Zahtjevi.Where(w => w.KorisnikId == logiraniKorisnik.KorisnikId && w.ZahtjevStatusId == 2).Count();
                    model.BrojArhiviranihZahtjeva = _context.Zahtjevi.Where(w => w.KorisnikId == logiraniKorisnik.KorisnikId && w.ZahtjevStatusId == 3).Count();
                }

                else if (logiraniKorisnik.IsMenadzment)
                {
                    model.IsMenadzment = true;

                    model.BrojPrihvacenihPonuda = _context.Ponude.Where(w => w.PonudaStatus == true && w.DatumIzdavanja.Year == DateTime.Now.Year).Count();
                    model.BrojPrihvacenihPonudaMjesec = _context.Ponude.Where(w => w.PonudaStatus == true && w.DatumIzdavanja.Month == DateTime.Now.Month).Count();

                    var listPonuda = _context.Ponude.Where(w => w.PonudaStatus == true && w.DatumIzdavanja.Year == DateTime.Now.Year).ToList();
                    var listPonudaMjesec = _context.Ponude.Where(w => w.PonudaStatus == true && w.DatumIzdavanja.Month == DateTime.Now.Month).ToList();

                    foreach (var ponuda in listPonuda)
                    {
                        model.UkupanIznosBezPdv += ponuda.UkupanIznosBezPdv;
                        model.UkupanIznosSaPdv += ponuda.UkupanIznosSaPdv;
                    }

                    foreach (var ponuda in listPonudaMjesec)
                    {
                        model.UkupanIznosBezPdvMjesec += ponuda.UkupanIznosBezPdv;
                        model.UkupanIznosSaPdvMjesec += ponuda.UkupanIznosSaPdv;
                    }
                }

                else if (logiraniKorisnik.IsAdministrator)
                {
                    model.IsAdministrator = true;

                    var korisnici = _context.Korisnici.OrderByDescending(o => o.KorisnikId).Take(8).ToList();

                    model.KorisniciPodaci = new List<HomeIndexVM.KorisnikPodaci>();

                    foreach (var korisnik in korisnici)
                    {
                        var x = ImageHelper.GetImageType(korisnik.KorisnikSlika);

                        var korisnikPodaci = new HomeIndexVM.KorisnikPodaci()
                        {
                            ImePrezimeKorisnika = korisnik.Ime + " " + korisnik.Prezime,
                            KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(korisnik.KorisnikSlika))
                        };

                        model.KorisniciPodaci.Add(korisnikPodaci);
                    }
                }
            }

            return View(model);
        }
    }
}
