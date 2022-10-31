using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class BaseController : Controller
    {
        // this controller will be inherited from all other controllers 

        protected AppDbContext Db => (AppDbContext)HttpContext.RequestServices.GetService(typeof(AppDbContext))!;

        // load from Database
        protected User GetUser(HttpRequest request)
        {
            string key = "UserID";
            var userId = request.Cookies[key];
            User user = Db.Users
                .Include(u => u.DailyTasks)
                .Include(u => u.Preference)
                .Include(u => u.Bank)
                .ThenInclude(b => b.Operations)
                .Single(u => u.UserId == userId);
            return user;
        }

        // load from Database but using user id
        protected User GetUser(string userId)
        {
            User user = Db.Users
                .Include(u => u.DailyTasks)
                .Include(u => u.Preference)
                .Include(u => u.Bank)
                .ThenInclude(b => b.Operations)
                .Single(u => u.UserId == userId);
            return user;
        }

        // to avoid errors
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

        protected IActionResult? CheckUserExist(string userId)
        {
            if (Db.Users.Find(userId) == null)
            {
                TempData["LoginError"] = "لم يتم العثور على الحساب";
                return RedirectToAction("Login", "Welcome");
            }
            return null;
        }
    }
}
