using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.Util.Prijava;
using ServisApp.ViewModels;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: true, org: true, ing: true, mgmt: true, client: true)]
    public class SesijaController : Controller
    {
        private readonly MojContext _context;
        public SesijaController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var trenutniKorisnik = HttpContext.GetLogiraniKorisnik();

            SesijaIndexVM model = new SesijaIndexVM();

            if (trenutniKorisnik != null && trenutniKorisnik.KlijentskiRacunId != 0)
            {
                model.Rows = _context.AutorizacijskiTokeni.Where(w => w.KlijentskiRacunId == trenutniKorisnik.KlijentskiRacunId).Select(s => new SesijaIndexVM.Row
                {
                    VrijemeLogiranja = s.VrijemeEvidentiranja,
                    token = s.Vrijednost,
                    IpAdresa = s.IpAdresa
                }).ToList();

                model.TrenutniToken = HttpContext.GetTrenutniToken();
            }
            else if (trenutniKorisnik != null && trenutniKorisnik.KorisnikId != 0)
            {
                model.Rows = _context.AutorizacijskiTokeni.Where(w => w.KorisnikId == trenutniKorisnik.KorisnikId).Select(s => new SesijaIndexVM.Row
                {
                    VrijemeLogiranja = s.VrijemeEvidentiranja,
                    token = s.Vrijednost,
                    IpAdresa = s.IpAdresa
                }).ToList();

                model.TrenutniToken = HttpContext.GetTrenutniToken();
            }

            return View(model);
        }

        public IActionResult Obrisi(string token)
        {
            AutorizacijskiToken obrisati = _context.AutorizacijskiTokeni.FirstOrDefault(x => x.Vrijednost == token);

            if (obrisati != null)
            {
                _context.AutorizacijskiTokeni.Remove(obrisati);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}