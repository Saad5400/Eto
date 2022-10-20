using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class SettingsController : BaseController
    {
        public IActionResult Index()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);
            return View(user);
        }
        public IActionResult SwitchTheme()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);
            if (user.Preference.Theme == "DarkBlue")
            {
                user.Preference.Theme = "LightOrange";
            }
            else
            {
                user.Preference.Theme = "DarkBlue";
            }
            _db.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
