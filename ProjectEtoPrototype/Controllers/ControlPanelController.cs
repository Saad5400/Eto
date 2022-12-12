using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class ControlPanelController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
            var b = new BaseModel { AllUsers = Db.Users
                                                // .Include(u => u.Preference)
                                                // .Include(u => u.DailyTasks)
                                                // .Include(u => u.Bank)
                                                // .ThenInclude(b => b.Operations)
                                                .ToList() };
            return View(b);
        }

        public IActionResult UserData(string userId)
        {
            return RedirectToAction("Index", "Home");
            User user = GetUser(userId);
            return View(user);
        }
    }
}
