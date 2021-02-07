using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Data;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: true, org: true, ing: true, mgmt: true, client: false)]
    public class KorisnickiProfilController : Controller
    {
        private readonly MojContext _context;
        public KorisnickiProfilController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Uredi(int id)
        {
            AutentifikacijaVM logiraniKorisnik = HttpContext.GetLogiraniKorisnik();

            var korisnik = _context.Korisnici.Find(id);

            if (korisnik == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            if (logiraniKorisnik.KorisnikId != korisnik.KorisnikId)
            {
                TempData["error_poruka"] = "Nemate pravo pristupa";
                return RedirectToAction("Index", "Autentifikacija", new { @area = "" });
            }

            var model = new KorisnickiProfilUredi()
            {
                KorisnikId = korisnik.KorisnikId,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                KorisnickoIme = korisnik.KorisnickoIme
            };

            var x = ImageHelper.GetImageType(korisnik.KorisnikSlika);
            model.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(korisnik.KorisnikSlika));

            return View(model);
        }

        public IActionResult Snimi(KorisnickiProfilUredi input)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", input);
            }

            var lozinkaSalt = Util.Helper.GenerateSalt();
            var lozinkaHash = Util.Helper.GenerateHash(lozinkaSalt, input.Lozinka);

            var korisnik = _context.Korisnici.Find(input.KorisnikId);

            if (korisnik != null)
            {
                korisnik.LozinkaHash = lozinkaHash;
                korisnik.LozinkaSalt = lozinkaSalt;

                if (input.KorisnikSlika != null)
                {
                    using var stream = new MemoryStream();
                    input.KorisnikSlika.CopyTo(stream);
                    korisnik.KorisnikSlika = stream.ToArray();
                }

                _context.SaveChanges();
            }

            return Redirect("/Home/Index");
        }
    }
}