using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ProjectEtoPrototype.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SwitchTheme()
        {
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
