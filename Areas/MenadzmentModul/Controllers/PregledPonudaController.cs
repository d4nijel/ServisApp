using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.MenadzmentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: true, client: false)]
    [Area("MenadzmentModul")]
    public class PregledPonudaController : Controller
    {
        private readonly MojContext _context;
        public PregledPonudaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult PonudaPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index()
        {
            var model = new PonudaIndexVM
            {
                Rows = _context.Ponude.Select(s => new PonudaIndexVM.Row
                {
                    PonudaId = s.PonudaId,
                    BrojPonude = s.BrojPonude,
                    DatumIzdavanja = s.DatumIzdavanja.Date.ToString("dd.MM.yyyy"),
                    NazivKlijenta = s.Klijent.Naziv,
                    UkupanIznosSaPDV = s.UkupanIznosSaPdv,
                    PonudaStatus = s.PonudaStatus
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var ponuda = _context.Ponude.Where(w => w.PonudaId == id).Include(i => i.Klijent).ThenInclude(t => t.Mjesto).Include(n => n.Korisnik).SingleOrDefault();

            if (ponuda == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new PonudaDetaljiVM
            {
                BrojPonude = ponuda.BrojPonude,
                NazivKlijenta = ponuda.Klijent.Naziv + ", ul. " + ponuda.Klijent.Ulica + ", " + ponuda.Klijent.Mjesto.Naziv,
                DatumIzdavanja = ponuda.DatumIzdavanja.ToString("dd.MM.yyyy"),
                PonudaStatus = ponuda.PonudaStatus,
                Korisnik = ponuda.Korisnik.Ime + " " + ponuda.Korisnik.Prezime,
                UkupanIznosBezPdv = ponuda.UkupanIznosBezPdv,
                PDV = ponuda.UkupanIznosSaPdv - ponuda.UkupanIznosBezPdv,
                UkupanIznosSaPdv = ponuda.UkupanIznosSaPdv,
                PonudaPath = ponuda.PonudaPath
            };

            return View(model);
        }
    }
}