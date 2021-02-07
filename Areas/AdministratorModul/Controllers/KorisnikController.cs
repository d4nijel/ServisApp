using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.AdministratorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.AdministratorModul.Controllers
{
    [Autorizacija(admin: true, org: false, ing: false, mgmt: false, client: false)]
    [Area("AdministratorModul")]
    public class KorisnikController : Controller
    {
        private readonly MojContext _context;
        public KorisnikController(MojContext context)
        {
            _context = context;
        }

        public IActionResult ProvjeraKorisnickogImena(string korisnickoIme)
        {
            if (_context.Korisnici.Any(a => a.KorisnickoIme.ToUpper() == korisnickoIme.ToUpper()) || _context.KlijentskiRacuni.Any(a => a.KorisnickoIme.ToUpper() == korisnickoIme.ToUpper()))
            {
                return Json($"Korisničko ime \"{korisnickoIme}\" se već nalazi u bazi");
            }

            return Json(true);
        }

        private void GenerisiUloge(KorisnikDodajVM model)
        {
            model.Uloge = _context.Uloge.Select(s => new SelectListItem
            {
                Value = s.UlogaId.ToString(),
                Text = s.Naziv
            }).ToList();
        }

        private void GenerisiUlogeDetalji(KorisnikDetaljiVM model)
        {
            model.Uloge = _context.Uloge.Select(s => new SelectListItem
            {
                Value = s.UlogaId.ToString(),
                Text = s.Naziv
            }).ToList();
        }

        public IActionResult Index()
        {
            var model = new KorisnikIndexVM
            {
                Rows = _context.Korisnici.Select(s => new KorisnikIndexVM.Row()
                {
                    KorisnikId = s.KorisnikId,
                    ImePrezime = s.Ime + " " + s.Prezime,
                    KorisnickoIme = s.KorisnickoIme,
                    KorisnikSlika = s.KorisnikSlika,
                    KorisnikStatus = s.KorisnikStatus,
                    DeleteBtn = ((s.Permisije.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null &&
                    (s.Zahtjevi.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null &&
                    (s.Poruke.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null &&
                    (s.Ugovori.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null &&
                    (s.Ponude.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null &&
                    (s.Dokumenti.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null &&
                    (s.Izvjestaj.Where(w => w.KorisnikId == s.KorisnikId).SingleOrDefault()) == null) ? true : false
                }).ToList()
            };

            foreach (var item in model.Rows)
            {
                var x = ImageHelper.GetImageType(item.KorisnikSlika);
                item.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(item.KorisnikSlika));
            }

            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new KorisnikDodajVM();

            GenerisiUloge(model);

            return View(model);
        }

        public IActionResult Snimi(KorisnikDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiUloge(input);
                return View("Dodaj", input);
            }

            var lozinkaSalt = Util.Helper.GenerateSalt();
            var lozinkaHash = Util.Helper.GenerateHash(lozinkaSalt, input.Lozinka);

            Korisnik noviKorisnik = new Korisnik()
            {
                Ime = input.Ime,
                Prezime = input.Prezime,
                KorisnickoIme = input.KorisnickoIme,
                LozinkaHash = lozinkaHash,
                LozinkaSalt = lozinkaSalt,
                KorisnikStatus = true
            };

            using (var stream = new MemoryStream())
            {
                input.KorisnikSlika.CopyTo(stream);
                noviKorisnik.KorisnikSlika = stream.ToArray();
            }

            _context.Korisnici.Add(noviKorisnik);
            _context.SaveChanges();

            var UlogeIds = input.Uloge.Where(w => w.Selected == true).Select(s => s.Value);

            foreach (var id in UlogeIds)
            {
                Permisija p = new Permisija
                {
                    DatumIzmjene = DateTime.Now,
                    KorisnikId = noviKorisnik.KorisnikId,
                    UlogaId = int.Parse(id),
                    PermisijaStatus = true
                };
                _context.Permisije.Add(p);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Detalji(int id)
        {
            var korisnik = _context.Korisnici.Find(id);

            if (korisnik == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new KorisnikDetaljiVM()
            {
                KorisnikId = korisnik.KorisnikId,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                KorisnickoIme = korisnik.KorisnickoIme,
                KorisnikStatus = korisnik.KorisnikStatus
            };

            var x = ImageHelper.GetImageType(korisnik.KorisnikSlika);
            model.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(korisnik.KorisnikSlika));

            GenerisiUlogeDetalji(model);

            var ulogeList = _context.Permisije.Where(w => w.KorisnikId == korisnik.KorisnikId && w.PermisijaStatus == true).Select(s => s.UlogaId).ToList();

            foreach (var oznaceneUloge in ulogeList)
            {
                foreach (var uloga in model.Uloge)
                {
                    if (int.Parse(uloga.Value) == oznaceneUloge)
                    {
                        uloga.Selected = true;
                    }
                }
            }

            return View(model);
        }

        public IActionResult Edit(KorisnikDetaljiVM input)
        {
            if (!ModelState.IsValid)
            {
                GenerisiUlogeDetalji(input);
                return View("Detalji", input);
            }

            var lozinkaSalt = Util.Helper.GenerateSalt();
            var lozinkaHash = Util.Helper.GenerateHash(lozinkaSalt, input.Lozinka);

            var korisnik = _context.Korisnici.Find(input.KorisnikId);

            if (korisnik != null)
            {
                korisnik.Ime = input.Ime;
                korisnik.Prezime = input.Prezime;
                korisnik.LozinkaHash = lozinkaHash;
                korisnik.LozinkaSalt = lozinkaSalt;
                korisnik.KorisnikStatus = input.KorisnikStatus;

                if (input.KorisnikSlika != null)
                {
                    using var stream = new MemoryStream();
                    input.KorisnikSlika.CopyTo(stream);
                    korisnik.KorisnikSlika = stream.ToArray();
                }

                var UlogeIds = input.Uloge.Where(w => w.Selected).Select(s => s.Value);

                var permisije = _context.Permisije.Where(w => w.KorisnikId == korisnik.KorisnikId && w.PermisijaStatus == true);

                foreach (var permisija in permisije)
                {
                    permisija.PermisijaStatus = false;
                }

                foreach (var id in UlogeIds)
                {
                    Permisija p = new Permisija
                    {
                        DatumIzmjene = DateTime.Now,
                        KorisnikId = korisnik.KorisnikId,
                        UlogaId = int.Parse(id),
                        PermisijaStatus = true
                    };
                    _context.Permisije.Add(p);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var korisnik = _context.Korisnici.Find(id);

            if (korisnik == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            _context.Korisnici.Remove(korisnik);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}