using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    public class AutentifikacijaController : Controller
    {
        private readonly MojContext _context;
        public AutentifikacijaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new AutentifikacijaVM
            {
                ZapamtiLozinku = false
            };

            return View(model);
        }

        public IActionResult Prijava(AutentifikacijaVM input)
        {
            bool korisnikLogin = false;
            bool klijentskiRacunLogin = false;

            Korisnik korisnik = _context.Korisnici.Where(w => w.KorisnickoIme == input.KorisnickoIme && w.KorisnikStatus == true).SingleOrDefault();

            if (korisnik != null)
            {
                var lozinkaHashKorisnik = Util.Helper.GenerateHash(korisnik.LozinkaSalt, input.Lozinka);

                if (korisnik.LozinkaHash == lozinkaHashKorisnik)
                {
                    korisnikLogin = true;
                }

                if (korisnikLogin)
                {
                    input.ImePrezime = korisnik.Ime + " " + korisnik.Prezime;
                    input.KorisnikId = korisnik.KorisnikId;

                    var ulogeList = _context.Permisije.Where(w => w.KorisnikId == korisnik.KorisnikId && w.PermisijaStatus == true).Select(s => s.UlogaId).ToList();

                    if (ulogeList != null)
                    {
                        foreach (var item in ulogeList)
                        {
                            //provjeravamo da li je logirani korisnik Administrator, Organizator, Inzinjer, Menadžment
                            switch (item)
                            {
                                case 1:
                                    input.IsAdministrator = true;
                                    break;
                                case 2:
                                    input.IsOrganizator = true;
                                    break;
                                case 3:
                                    input.IsInzinjer = true;
                                    break;
                                case 4:
                                    input.IsMenadzment = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    var x = ImageHelper.GetImageType(korisnik.KorisnikSlika);
                    input.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(korisnik.KorisnikSlika));
                }
            }

            else
            {
                KlijentskiRacun klijentskiRacun = _context.KlijentskiRacuni.Where(w => w.KorisnickoIme == input.KorisnickoIme && w.KlijentskiRacunStatus == true).SingleOrDefault();

                if (klijentskiRacun != null)
                {
                    var lozinkaHashKlijentskiRacun = Util.Helper.GenerateHash(klijentskiRacun.LozinkaSalt, input.Lozinka);

                    if (klijentskiRacun.LozinkaHash == lozinkaHashKlijentskiRacun)
                    {
                        klijentskiRacunLogin = true;
                    }

                    if (klijentskiRacunLogin)
                    {
                        input.ImePrezime = klijentskiRacun.Ime + " " + klijentskiRacun.Prezime;
                        input.KlijentskiRacunId = klijentskiRacun.KlijentskiRacunId;
                        input.IsKlijent = true;

                        var x = ImageHelper.GetImageType(klijentskiRacun.KlijentskiRacunSlika);
                        input.KlijentskiRacunSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(klijentskiRacun.KlijentskiRacunSlika));
                    }
                }
            }

            if ((korisnikLogin == true && klijentskiRacunLogin == true) || (korisnikLogin == false && klijentskiRacunLogin == false))
            {
                TempData["error_poruka"] = "Pogrešno korisničko ime, lozinka ili neaktivan račun";
                return View("Index", input);
            }

            HttpContext.SetLogiraniKorisnik(input, input.ZapamtiLozinku);

            return Redirect("/Home/Index");
        }

        public IActionResult Odjava()
        {
            HttpContext.Session.Remove("logirani_korisnik");

            return RedirectToAction("Index");
        }
    }
}