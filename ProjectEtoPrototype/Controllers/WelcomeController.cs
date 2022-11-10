using Microsoft.AspNetCore.Mvc;
using ProjectEtoPrototype.Models;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Classes;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;

namespace ProjectEtoPrototype.Controllers
{
    public class WelcomeController : BaseController
    {
        public IActionResult Index()
        {
            if (Request.Cookies["UserID"] is not null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET
        public IActionResult Register()
        {
            // how many accounts were created on this account
            var countKey = "accountsCount";
            var count = Convert.ToInt32(Request.Cookies[countKey]);

            var pastUsersId = new string?[] { 
                Request.Cookies["UserId1"],
                Request.Cookies["UserId2"],
                Request.Cookies["UserId3"]
            };

            // prevent user from creating more than 3 accounts
            if (count >= 3)
            {
                TempData["LoginError"] = "ما تقدر تسجل اكثر من 3 حسابات لكل جهاز";
                return RedirectToAction("Login", "Welcome");
            }

            // use the id manager to generate an id
            string userId;
            do
            { 
                // keep generating until getting a new id that doesn't exist
                userId = IdManager.GenerateNewId();
            } while (Db.Users.Find(userId) is not null);
            
            CookieOptions cookieOptions = new CookieOptions{ Expires = DateTime.Now.AddYears(1) };

            // create new user object
            User user = new User{ UserId = userId };

            user.DailyTasks.Add(new DailyTask
            {
                Name = "اكمال الواجب", UserId = user.UserId, CreatedDate = DateTime.Now, User = user
            });
            user.Bank.Operations.Add(new Operation
            {
                Description = "البقالة", Class = "مقاضي", Amount = -5, Bank = user.Bank, 
                CreatedDate = DateTime.Now, BankId = user.Bank.BankId
            });
            user.Bank.Balance = -5;

            // add that user object to database
            Db.Add(user);
            Db.SaveChanges();

            // adding new registered user id to the list
            pastUsersId[count] = userId;
            count += 1;

            // adding user id to cookies
            Response.Cookies.Append("UserID", userId, cookieOptions);
            // theme
            Response.Cookies.Append("Theme", "LightOrange", cookieOptions);
            // how many accounts
            Response.Cookies.Append(countKey, count.ToString()!, cookieOptions);

            // adding each account id
            for (int i = 0; i < 3; i++)
            {
                if (pastUsersId[i] is null) { continue; }
                Response.Cookies.Append($"UserId{i+1}", pastUsersId[i]!, cookieOptions);
            }

            return RedirectToAction("RegisterPage", "Welcome", user);
        }

        public IActionResult RegisterPage(User user)
        {
            // just a linking so that when user refresh the page no new accounts are created
            return View("Register", user);
        }

        // GET
        public IActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User? user)
        {
            // some checking. checking if account exist
            if (user is null)
            {
                TempData["LoginError"] = "يجب ادخال الايدي";
                return View(user);
            }
            if (string.IsNullOrEmpty(user.UserId))
            {
                TempData["LoginError"] = "يجب ادخال الايدي";
                return View(user);
            }
            if (user.UserId.Length < 9)
            {
                TempData["LoginError"] = "الايدي غير صحيح";
                return View(user);
            }
            var exist = CheckUserExist(user.UserId);
            if (exist is not null) { return exist; }

            // just so we can add the theme to cookies
            user.Preference = Db.Preferences.First(p => p.UserId == user.UserId);
            
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
            };
            Response.Cookies.Append("UserID", user.UserId, cookieOptions);
            Response.Cookies.Append("Theme", user.Preference.Theme, cookieOptions);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult HideUserId(int order)
        {
            // remove one of the IDs from اخر الحسابات
            order++;
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            };
            Response.Cookies.Append($"UserId{order}", String.Empty, cookieOptions);

            return RedirectToAction("Login", "Welcome");
        }

        public IActionResult Logout()
        {
            // remove user id from cookies
            string key = "UserID";
            string value = String.Empty;
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            };
            Response.Cookies.Append(key, value, cookieOptions);
            return RedirectToAction("Index", "Welcome");
        }
    }
}
