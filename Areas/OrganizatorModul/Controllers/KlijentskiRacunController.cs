using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.OrganizatorModul.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: false, client: false)]
    [Area("OrganizatorModul")]
    public class KlijentskiRacunController : Controller
    {
        private readonly MojContext _context;
        public KlijentskiRacunController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraEmailaKlijentskogRacuna(string email, int klijentskiRacunId)
        {
            if (klijentskiRacunId == 0)
            {
                if (_context.KlijentskiRacuni.Any(a => a.Email.ToUpper() == email.ToUpper()))
                {
                    return Json($"Email adresa \"{email}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.KlijentskiRacuni.Any(a => a.Email.ToUpper() == email.ToUpper() && a.KlijentskiRacunId != klijentskiRacunId))
                {
                    return Json($"Email adresa \"{email}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult ProvjeraKorisnickogImenaKlijentskogRacuna(string korisnickoIme, int klijentskiRacunId)
        {
            if (klijentskiRacunId == 0)
            {
                if (_context.Korisnici.Any(a => a.KorisnickoIme.ToUpper() == korisnickoIme.ToUpper()) || _context.KlijentskiRacuni.Any(a => a.KorisnickoIme.ToUpper() == korisnickoIme.ToUpper()))
                {
                    return Json($"Korisničko ime \"{korisnickoIme}\" se već nalazi u bazi");
                }
            }
            else
            {
                if (_context.KlijentskiRacuni.Any(a => a.KorisnickoIme.ToUpper() == korisnickoIme.ToUpper() && a.KlijentskiRacunId != klijentskiRacunId))
                {
                    return Json($"Korisničko ime \"{korisnickoIme}\" se već nalazi u bazi");
                }
            }
            return Json(true);
        }

        public IActionResult Dodaj(int id)
        {
            var klijent = _context.Klijenti.Find(id);

            if (klijent == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new KlijentskiRacunDodajVM()
            {
                KlijentId = klijent.KlijentId,
                Klijent = klijent.SkraceniNaziv + " - " + klijent.IdBroj
            };

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var klijentskiRacun = _context.KlijentskiRacuni.Where(w => w.KlijentskiRacunId == id).Include(i => i.Klijent).SingleOrDefault();

            if (klijentskiRacun == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new KlijentskiRacunDetaljiVM()
            {
                KlijentskiRacunId = klijentskiRacun.KlijentskiRacunId,
                Ime = klijentskiRacun.Ime,
                Prezime = klijentskiRacun.Prezime,
                Email = klijentskiRacun.Email,
                KorisnickoIme = klijentskiRacun.KorisnickoIme,
                EmailNotifikacija = klijentskiRacun.EmailNotifikacija,
                BrojDanaPrijeIsteka = klijentskiRacun.BrojDanaPrijeIsteka,
                KlijentskiRacunStatus = klijentskiRacun.KlijentskiRacunStatus,
                Klijent = klijentskiRacun.Klijent.SkraceniNaziv + " - " + klijentskiRacun.Klijent.IdBroj
            };

            var x = ImageHelper.GetImageType(klijentskiRacun.KlijentskiRacunSlika);
            model.KlijentskiRacunSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(klijentskiRacun.KlijentskiRacunSlika));

            return View(model);
        }

        public IActionResult Snimi(KlijentskiRacunDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            var lozinkaSalt = Util.Helper.GenerateSalt();
            var lozinkaHash = Util.Helper.GenerateHash(lozinkaSalt, input.Lozinka);

            KlijentskiRacun noviKlijentskiRacun = new KlijentskiRacun()
            {
                Ime = input.Ime,
                Prezime = input.Prezime,
                DatumRegistracije = DateTime.Now,
                Email = input.Email,
                KorisnickoIme = input.KorisnickoIme,
                LozinkaHash = lozinkaHash,
                LozinkaSalt = lozinkaSalt,
                EmailNotifikacija = true,
                BrojDanaPrijeIsteka = 20,
                KlijentskiRacunStatus = true,
                KlijentId = input.KlijentId
            };

            using (var stream = new MemoryStream())
            {
                input.KlijentskiRacunSlika.CopyTo(stream);
                noviKlijentskiRacun.KlijentskiRacunSlika = stream.ToArray();
            }

            _context.KlijentskiRacuni.Add(noviKlijentskiRacun);

            _context.SaveChanges();

            return RedirectToAction("Index", "Klijent");
        }

        public IActionResult Edit(KlijentskiRacunDetaljiVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Detalji", input);
            }

            var lozinkaSalt = Util.Helper.GenerateSalt();
            var lozinkaHash = Util.Helper.GenerateHash(lozinkaSalt, input.Lozinka);

            var stariKlijentskiRacun = _context.KlijentskiRacuni.Find(input.KlijentskiRacunId);

            if (stariKlijentskiRacun != null)
            {
                stariKlijentskiRacun.Ime = input.Ime;
                stariKlijentskiRacun.Prezime = input.Prezime;
                stariKlijentskiRacun.Email = input.Email;
                stariKlijentskiRacun.LozinkaHash = lozinkaHash;
                stariKlijentskiRacun.LozinkaSalt = lozinkaSalt;
                stariKlijentskiRacun.EmailNotifikacija = input.EmailNotifikacija;
                stariKlijentskiRacun.BrojDanaPrijeIsteka = input.BrojDanaPrijeIsteka;
                stariKlijentskiRacun.KlijentskiRacunStatus = input.KlijentskiRacunStatus;

                if (input.KlijentskiRacunSlika != null)
                {
                    using var stream = new MemoryStream();
                    input.KlijentskiRacunSlika.CopyTo(stream);

                    stariKlijentskiRacun.KlijentskiRacunSlika = stream.ToArray();
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Klijent");
        }

        public IActionResult Obrisi(int id)
        {
            var klijentskiRacun = _context.KlijentskiRacuni.Find(id);

            if (klijentskiRacun == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.KlijentskiRacuni.Remove(klijentskiRacun);
            _context.SaveChanges();

            return RedirectToAction("Index", "Klijent");
        }
    }
}