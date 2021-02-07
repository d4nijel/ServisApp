using Microsoft.AspNetCore.Mvc;
using ServisApp.Data;
using ServisApp.Util.Prijava;

namespace ServisApp.Controllers
{
    [Autorizacija(admin: false, org: true, ing: true, mgmt: false, client: false)]
    public class ArhivirajZahtjevController : Controller
    {
        private readonly MojContext _context;
        public ArhivirajZahtjevController(MojContext context)
        {
            _context = context;
        }

        public IActionResult Zakljucaj(int id)
        {
            var zahtjev = _context.Zahtjevi.Find(id);

            if (zahtjev == null)
            {
                Response.StatusCode = 404;
                return View("Views/Shared/Error404.cshtml");
            }

            zahtjev.ZahtjevStatusId = 3;

            _context.SaveChanges();

            return Redirect("/Podrska/Detalji?Id=" + zahtjev.ZahtjevId);

        }
    }
}