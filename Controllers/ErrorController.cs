using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServisApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return View("Views/Shared/Error404.cshtml");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View("Views/Shared/Error500.cshtml");
        }
    }
}