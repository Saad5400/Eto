using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
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
            CookieOptions cookieOptions = new CookieOptions { Expires = DateTime.Now.AddYears(1) };

            if (user.Preference.Theme == "DarkBlue")
            {
                user.Preference.Theme = "LightOrange";
                Response.Cookies.Append("Theme", "LightOrange", cookieOptions);
            }
            else
            {
                user.Preference.Theme = "DarkBlue";
                Response.Cookies.Append("Theme", "DarkBlue", cookieOptions);
            }
            Db.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
