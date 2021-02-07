using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServisApp.Areas.MenadzmentModul.ViewModels;
using ServisApp.Data;
using ServisApp.Util.Prijava;

namespace ServisApp.Areas.OrganizatorModul.Controllers
{
    [Autorizacija(admin: false, org: true, ing: false, mgmt: false, client: false)]
    [Area("OrganizatorModul")]
    public class PregledUgovoraController : Controller
    {
        private readonly MojContext _context;
        public PregledUgovoraController(MojContext context)
        {
            _context = context;
        }

        public IActionResult UgovorPDF(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), file);

            var dfile = new PhysicalFileResult(path, "application/pdf");

            return dfile;
        }

        public IActionResult Index()
        {
            var ugovori = _context.Ugovori.Where(w => w.UgovorStatus == true && DateTime.Now.Date >= w.DatumIsteka.Date).ToList();

            foreach (var item in ugovori)
            {
                item.UgovorStatus = false;
            }
            _context.SaveChanges();

            var model = new UgovorIndexVM()
            {
                Rows = _context.Ugovori.Select(s => new UgovorIndexVM.Row
                {
                    UgovorId = s.UgovorId,
                    BrojUgovora = s.BrojUgovora,
                    Naziv = s.Naziv,
                    Klijent = s.Klijent.Naziv,
                    DatumPotpisivanja = s.DatumPotpisivanja.Date.ToString("dd.MM.yyyy"),
                    UgovorStatus = s.UgovorStatus
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            var ugovor = _context.Ugovori.Where(w => w.UgovorId == id).Include(i => i.Klijent).ThenInclude(t => t.Mjesto).SingleOrDefault();

            if (ugovor == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            var model = new UgovorDetaljiVM
            {
                BrojUgovora = ugovor.BrojUgovora,
                Naziv = ugovor.Naziv,
                DatumPotpisivanja = ugovor.DatumPotpisivanja.ToString("dd.MM.yyyy"),
                DatumIsteka = ugovor.DatumIsteka.ToString("dd.MM.yyyy"),
                UgovorPath = ugovor.UgovorPath,
                UgovorStatus = ugovor.UgovorStatus,
                Klijent = ugovor.Klijent.Naziv + ", ul. " + ugovor.Klijent.Ulica + ", " + ugovor.Klijent.Mjesto.Naziv
            };

            return View(model);
        }
    }
}