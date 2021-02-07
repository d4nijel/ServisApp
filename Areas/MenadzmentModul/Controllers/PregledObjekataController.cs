using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.OrganizatorModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;
using System.Linq;

namespace ServisApp.Areas.MenadzmentModul.Controllers
{
    [Autorizacija(admin: false, org: false, ing: false, mgmt: true, client: false)]
    [Area("MenadzmentModul")]
    public class PregledObjekataController : Controller
    {
        private readonly MojContext _context;
        public PregledObjekataController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new ObjekatIndexVM
            {
                Rows = _context.Objekti.Select(s => new ObjekatIndexVM.Row
                {
                    ObjekatId = s.ObjekatId,
                    Naziv = s.Naziv,
                    Lokacija = "ul. " + s.Ulica + ", " + s.Mjesto.Naziv,
                    Klijent = s.Klijent.SkraceniNaziv,
                    BrojRadnihNaloga = s.RadniNalozi.Count(),
                    ObjekatStatus = s.ObjekatStatus
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var objekat = _context.Objekti.Where(w => w.ObjekatId == id).Include(i => i.Klijent).Include(n => n.Mjesto).ThenInclude(t => t.Opcina).SingleOrDefault();

            if (objekat == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new ObjekatDetaljiVM()
            {
                Naziv = objekat.Naziv,
                Ulica = objekat.Ulica,
                KontaktOsoba = objekat.KontaktOsoba,
                KontaktBrojFiksni = objekat.KontaktBrojFiksni,
                KontaktBrojMobitel = objekat.KontaktBrojMobitel,
                KontaktEmail = objekat.KontaktEmail,
                ObjekatStatus = objekat.ObjekatStatus,
                NazivKlijenta = objekat.Klijent.Naziv,
                NazivMjesta = objekat.Mjesto.Naziv + ", općina " + objekat.Mjesto.Opcina.Naziv
            };

            return View(model);
        }
    }
}