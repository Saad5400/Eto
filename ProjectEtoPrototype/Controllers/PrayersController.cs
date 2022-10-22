using Microsoft.AspNetCore.Mvc;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class PrayersController : BaseController
    {
        public IActionResult Index()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            return View(user);
        }
    }
}
