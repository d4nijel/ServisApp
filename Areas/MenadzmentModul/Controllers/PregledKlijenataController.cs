using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.MenadzmentModul.ViewModels;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.MenadzmentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: true, client: false)]
    [Area("MenadzmentModul")]
    public class PregledKlijenataController : Controller
    {
        private readonly MojContext _context;
        public PregledKlijenataController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new KlijentIndexVM
            {
                Rows = _context.Klijenti.Select(s => new KlijentIndexVM.Row
                {
                    KlijentId = s.KlijentId,
                    Naziv = s.Naziv,
                    IdBroj = s.IdBroj,
                    Lokacija = "ul. " + s.Ulica + ", " + s.Mjesto.Naziv,
                    BrojObjekata = s.Objekti.Count(),
                    KlijentStatus = s.KlijentStatus
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var klijent = _context.Klijenti.Where(w => w.KlijentId == id).Include(i => i.Mjesto).ThenInclude(t => t.Opcina).SingleOrDefault();

            if (klijent == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new KlijentDetaljiVM
            {
                Naziv = klijent.Naziv,
                SkraceniNaziv = klijent.SkraceniNaziv,
                IdBroj = klijent.IdBroj,
                Ulica = klijent.Ulica,
                Sjediste = klijent.Mjesto.Naziv + ", općina " + klijent.Mjesto.Opcina.Naziv,
                KontaktOsoba = klijent.KontaktOsoba,
                KontaktBrojFiksni = klijent.KontaktBrojFiksni,
                KontaktBrojMobitel = klijent.KontaktBrojMobitel,
                KontaktEmail = klijent.KontaktEmail,
                KlijentStatus = klijent.KlijentStatus
            };

            return View(model);
        }
    }
}