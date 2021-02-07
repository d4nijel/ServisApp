using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.MenadzmentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Areas.MenadzmentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: true, client: false)]
    [Area("MenadzmentModul")]
    public class DokumentController : Controller
    {
        private readonly MojContext _context;
        public DokumentController(MojContext context)
        {
            _context = context;
        }

        public IActionResult DokumentPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index()
        {
            var model = new DokumentIndexVM
            {
                Rows = _context.Dokumenti.Select(s => new DokumentIndexVM.Row
                {
                    DokumentId = s.DokumentId,
                    Naziv = s.Naziv,
                    TipDokumenta = s.TipDokumenta,
                    DatumIzdavanja = s.DatumIzdavanja.ToString("dd.MM.yyyy"),
                    SluzbeniList = s.SluzbeniList,
                    DokumentPath = s.DokumentPath,
                    DokumentStatus = s.DokumentStatus,
                    Korisnik = s.Korisnik.Ime + " " + s.Korisnik.Prezime
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new DokumentDodajVM
            {
                DatumIzdavanja = DateTime.Now,
                DokumentStatus = true
            };

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var dokument = _context.Dokumenti.Where(w => w.DokumentId == id).Include(i => i.Korisnik).SingleOrDefault();

            if (dokument == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new DokumentDetaljiVM()
            {
                DokumentId = dokument.DokumentId,
                Naziv = dokument.Naziv,
                TipDokumenta = dokument.TipDokumenta,
                DatumIzdavanja = dokument.DatumIzdavanja.Date.ToString("dd.MM.yyyy"),
                SluzbeniList = dokument.SluzbeniList,
                DokumentPath = dokument.DokumentPath,
                DokumentStatus = dokument.DokumentStatus,
                Korisnik = dokument.Korisnik.Ime + " " + dokument.Korisnik.Prezime
            };
            return View(model);
        }

        public IActionResult Snimi(DokumentDodajVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", input);
            }

            AutentifikacijaVM korisnik = HttpContext.GetLogiraniKorisnik();

            Dokument noviDokument = new Dokument
            {
                Naziv = input.Naziv,
                TipDokumenta = input.TipDokumenta,
                DatumIzdavanja = input.DatumIzdavanja,
                SluzbeniList = input.PravniIzvor + " " + input.BrojSluzbenogGlasila,
                DokumentStatus = input.DokumentStatus,
                KorisnikId = korisnik.KorisnikId
            };

            noviDokument.DokumentPath = UploadDokumenata.UploadDoc(input.Dokument, input.TipDokumenta, UploadDokumenata.TipoviDokumenata.Dokumenti);

            _context.Dokumenti.Add(noviDokument);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(DokumentDetaljiVM input)
        {
            if (!ModelState.IsValid)
            {
                return View("Detalji", input);
            }

            var stariDokument = _context.Dokumenti.Find(input.DokumentId);

            if (stariDokument != null)
            {
                stariDokument.DokumentStatus = input.DokumentStatus;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int id)
        {
            var dokument = _context.Dokumenti.Find(id);

            if (dokument == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            UploadDokumenata.DeleteDoc(dokument.DokumentPath, true);

            _context.Dokumenti.Remove(dokument);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}