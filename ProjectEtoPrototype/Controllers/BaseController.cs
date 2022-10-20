using Microsoft.AspNetCore.Mvc;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class BaseController : Controller
    {
        protected AppDbContext _db => (AppDbContext)HttpContext.RequestServices.GetService(typeof(AppDbContext));
        protected User GetUser(HttpRequest request)
        {
            string key = "UserID";
            var userId = request.Cookies[key];
            User user = _db.Users.Find(userId);
            user.Preference = _db.Preferences.Find(userId);

            var dailyTasks = from obj in _db.DailyTasks
                              where obj.UserId == user.UserId
                              select obj;
            foreach (var obj in dailyTasks)
            {
                if (24 - (DateTime.Now - obj.CreatedDate).TotalHours <= 0)
                {
                    _db.DailyTasks.Remove(obj);
                    continue;
                }
                if (!user.DailyTasks.Contains(obj))
                    user.DailyTasks.Add(obj);
            }
            _db.SaveChanges();
            return user;
        }
        protected IActionResult? CheckUserExist(HttpRequest request)
        {
            string key = "UserID";
            var userId = request.Cookies[key];
            if (_db.Users.Find(userId) == null)
            {
                TempData["LoginError"] = "لم يتم العثور على الحساب";
                return RedirectToAction("Login", "Welcome");
            }
            if (_db.Preferences.Find(userId) == null)
            {
                TempData["LoginError"] = "هناك مشكلة بالحساب";
                return RedirectToAction("Login", "Welcome");
            }
            return null;
        }
    }
}
