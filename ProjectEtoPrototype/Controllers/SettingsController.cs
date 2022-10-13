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
            if (Program.GlobalVars["Theme"] == "bootswatchThemeDark.css")
            {
                Program.GlobalVars["Theme"] = "bootswatchThemeLight.css";
                Program.GlobalVars["ThemeIcon"] = "bi-moon";
            }
            else
            {
                Program.GlobalVars["Theme"] = "bootswatchThemeDark.css";
                Program.GlobalVars["ThemeIcon"] = "bi-brightness-high";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
