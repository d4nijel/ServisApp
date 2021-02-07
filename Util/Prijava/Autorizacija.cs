using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServisApp.Data;
using ServisApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ServisApp.Util.Prijava
{
    public class AutorizacijaAttribute : TypeFilterAttribute
    {
        public AutorizacijaAttribute(bool admin, bool org, bool ing, bool mgmt, bool client) : base(typeof(MyAuthorizeImpl))
        {
            Arguments = new object[] { admin, org, ing, mgmt, client };
        }
    }

    public class MyAuthorizeImpl : IAsyncActionFilter
    {
        public MyAuthorizeImpl(bool admin, bool org, bool ing, bool mgmt, bool client)
        {
            _administrator = admin;
            _organizator = org;
            _inzinjer = ing;
            _menadzment = mgmt;
            _klijent = client;
        }

        private readonly bool _administrator;
        private readonly bool _organizator;
        private readonly bool _inzinjer;
        private readonly bool _menadzment;
        private readonly bool _klijent;

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            AutentifikacijaVM logiraniKorisnik = filterContext.HttpContext.GetLogiraniKorisnik();

            if (logiraniKorisnik == null)
            {
                if (filterContext.Controller is Controller controller)
                {
                    controller.TempData["error_poruka"] = "Niste se prijavili";
                }

                filterContext.Result = new RedirectToActionResult("Index", "Autentifikacija", new { @area = "" });
                return;
            }

            //Preuzimamo DbContext preko app services
            MojContext db = filterContext.HttpContext.RequestServices.GetService<MojContext>();

            //provjeravamo da li je logirani korisnik Administrator, Organizator, Inzinjer, Menadžment
            if (logiraniKorisnik.IsKlijent)
            {
                if (_klijent && db.KlijentskiRacuni.Any(s => s.KlijentskiRacunId == logiraniKorisnik.KlijentskiRacunId))
                {
                    await next(); //ok - ima pravo pristupa
                    return;
                }
            }
            else
            {
                if (_administrator && db.Korisnici.Any(s => s.KorisnikId == logiraniKorisnik.KorisnikId) && logiraniKorisnik.IsAdministrator)
                {
                    await next(); //ok - ima pravo pristupa
                    return;
                }

                if (_organizator && db.Korisnici.Any(s => s.KorisnikId == logiraniKorisnik.KorisnikId) && logiraniKorisnik.IsOrganizator)
                {
                    await next(); //ok - ima pravo pristupa
                    return;
                }

                if (_inzinjer && db.Korisnici.Any(s => s.KorisnikId == logiraniKorisnik.KorisnikId) && logiraniKorisnik.IsInzinjer)
                {
                    await next(); //ok - ima pravo pristupa
                    return;
                }

                if (_menadzment && db.Korisnici.Any(s => s.KorisnikId == logiraniKorisnik.KorisnikId) && logiraniKorisnik.IsMenadzment)
                {
                    await next(); //ok - ima pravo pristupa
                    return;
                }
            }

            if (filterContext.Controller is Controller c1)
            {
                c1.TempData["error_poruka"] = "Nemate pravo pristupa";
            }

            filterContext.Result = new RedirectToActionResult("Index", "Autentifikacija", new { @area = "" });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // throw new NotImplementedException();
        }
    }
}
