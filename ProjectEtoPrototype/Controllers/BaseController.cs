using Microsoft.AspNetCore.Mvc;
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
            User user = Db.Users.Find(userId);
            user.Preference = Db.Preferences.Find(userId);

            var dailyTasks = from obj in Db.DailyTasks
                              where obj.UserId == user.UserId
                              select obj;
            foreach (var obj in dailyTasks)
            {
                if (24 - (DateTime.Now - obj.CreatedDate).TotalHours <= 0)
                {
                    Db.DailyTasks.Remove(obj);
                    continue;
                }
                if (!user.DailyTasks.Contains(obj))
                    user.DailyTasks.Add(obj);
            }
            Db.SaveChanges();
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
            if (Db.Preferences.Find(userId) == null)
            {
                TempData["LoginError"] = "هناك مشكلة بالحساب";
                return RedirectToAction("Login", "Welcome");
            }
            return null;
        }
    }
}
