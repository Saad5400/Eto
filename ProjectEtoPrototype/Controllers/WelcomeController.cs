using Microsoft.AspNetCore.Mvc;
using ProjectEtoPrototype.Models;
using ProjectEtoPrototype.Data;

namespace ProjectEtoPrototype.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET
        public IActionResult Login()
        {
            return View();
        }
    }
}
