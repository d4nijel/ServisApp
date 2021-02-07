using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.MenadzmentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: false, org: true, ing: true, mgmt: false, client: false)]
    public class PregledDokumenataController : Controller
    {
        private readonly MojContext _context;
        public PregledDokumenataController(MojContext context)
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
    }
}