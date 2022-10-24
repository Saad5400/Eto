using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class BaseController : Controller
    {
        protected AppDbContext Db => (AppDbContext)HttpContext.RequestServices.GetService(typeof(AppDbContext));

        protected User GetUser(HttpRequest request)
        {
            string key = "UserID";
            var userId = request.Cookies[key];
            User user = Db.Users
                .Include(u => u.DailyTasks)
                .Include(u => u.Preference)
                .Include(u => u.Bank)
                .ThenInclude(b => b.Operations)
                .First(u => u.UserId == userId);
            return user;
        }
        protected IActionResult? CheckUserExist(HttpRequest request)
        {
            string key = "UserID";
            var userId = request.Cookies[key];
            if (Db.Users.Find(userId) == null)
            {
                TempData["LoginError"] = "لم يتم العثور على الحساب";
                return RedirectToAction("Login", "Welcome");
            }
            return null;
        }
    }
}
