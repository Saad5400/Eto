using Microsoft.AspNetCore.Mvc;

namespace ProjectEtoPrototype.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
