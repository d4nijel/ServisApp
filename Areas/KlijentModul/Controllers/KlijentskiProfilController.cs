using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.KlijentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.KlijentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: false, client: true)]
    [Area("KlijentModul")]
    public class KlijentskiProfilController : Controller
    {
        private readonly MojContext _context;
        public KlijentskiProfilController(MojContext context)
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

        public IActionResult Uredi(int id)
        {
            AutentifikacijaVM logiraniKorisnik = HttpContext.GetLogiraniKorisnik();

            var klijentskiRacun = _context.KlijentskiRacuni.Where(w => w.KlijentskiRacunId == id).Include(i => i.Klijent).SingleOrDefault();

            if (klijentskiRacun == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            if (logiraniKorisnik.KlijentskiRacunId != klijentskiRacun.KlijentskiRacunId)
            {
                TempData["error_poruka"] = "Nemate pravo pristupa";
                return RedirectToAction("Index", "Autentifikacija", new { @area = "" });
            }

            var model = new KlijentskiRacunUrediVM()
            {
                KlijentskiRacunId = klijentskiRacun.KlijentskiRacunId,
                Ime = klijentskiRacun.Ime,
                Prezime = klijentskiRacun.Prezime,
                Email = klijentskiRacun.Email,
                KorisnickoIme = klijentskiRacun.KorisnickoIme,
                EmailNotifikacija = klijentskiRacun.EmailNotifikacija,
                BrojDanaPrijeIsteka = klijentskiRacun.BrojDanaPrijeIsteka
            };

            var x = ImageHelper.GetImageType(klijentskiRacun.KlijentskiRacunSlika);
            model.KlijentskiRacunSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(klijentskiRacun.KlijentskiRacunSlika));

            return View(model);
        }

        public IActionResult Edit(KlijentskiRacunUrediVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
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

                if (input.KlijentskiRacunSlika != null)
                {
                    using var stream = new MemoryStream();
                    input.KlijentskiRacunSlika.CopyTo(stream);

                    stariKlijentskiRacun.KlijentskiRacunSlika = stream.ToArray();
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home", new { @area = "" });
        }
    }
}