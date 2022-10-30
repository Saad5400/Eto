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
            if (Request.Cookies["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET
        public IActionResult Register()
        {
            string key = "UserID";
            string countKey = "accountsCount";
            int? count = Convert.ToInt32(Request.Cookies[countKey]);
            string?[] pastUsersId = new string?[] { 
                Request.Cookies["UserId1"],
                Request.Cookies["UserId2"],
                Request.Cookies["UserId3"]
            };

            if (count == null)
            {

            }
            else if (count >= 3)
            {
                TempData["LoginError"] = "ما تقدر تسجل اكثر من 3 حسابات لكل جهاز";
                return RedirectToAction("Login", "Welcome");
            }

            string userId;
            do
            {
                userId = IdManager.GenerateNewId();
            } while (Db.Users.Find(userId) != null);
            
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
            };
            User user = new User
            {
                UserId = userId
            };
            if (ModelState.IsValid)
            {
                Db.Add(user);
                Db.SaveChanges();
            }

            Response.Cookies.Append(key, userId, cookieOptions);
            Response.Cookies.Append("Theme", "LightOrange", cookieOptions);

            if (count == null || count == 0)
            {
                count = 1;
                Response.Cookies.Append(countKey, count.ToString(), cookieOptions);
                pastUsersId[0] = userId;
            }
            else if (count == 1)
            {
                count += 1;
                Response.Cookies.Append(countKey, count.ToString(), cookieOptions);
                pastUsersId[1] = userId;
            }
            else
            {
                count += 1;
                Response.Cookies.Append(countKey, count.ToString(), cookieOptions);
                pastUsersId[2] = userId;
            }

            for (int i = 0; i < 3; i++)
            {
                if (pastUsersId[i] == null) { continue; }
                Response.Cookies.Append($"UserId{i+1}", pastUsersId[i], cookieOptions);
            }

            return RedirectToAction("RegisterPage", "Welcome", user);
        }

        public IActionResult RegisterPage(User user)
        {
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
        public IActionResult Login(User user)
        {
            if (user == null)
            {
                TempData["LoginError"] = "يجب ادخال الايدي";
                return View(user);
            }
            if (user.UserId == null)
            {
                TempData["LoginError"] = "يجب ادخال الايدي";
                return View(user);
            }
            if (user.UserId.Length < 9)
            {
                TempData["LoginError"] = "الايدي غير صحيح";
                return View(user);
            }
            string key = "UserID";
            string value = user.UserId;
            user.Preference = Db.Preferences.First(p => p.UserId == user.UserId);
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
            };
            Response.Cookies.Append(key, value, cookieOptions);
            Response.Cookies.Append("Theme", user.Preference.Theme, cookieOptions);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult HideUserId(int order)
        {
            order++;
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            };
            Response.Cookies.Append($"UserId{order}", String.Empty, cookieOptions);

            return RedirectToAction("Login", "Welcome");
        }

        // GET
        public IActionResult Logout()
        {
            string key = "UserID";
            string value = String.Empty;
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            };
            Response.Cookies.Append(key, value, cookieOptions);
            return RedirectToAction("Index", "Welcome");
        }

        public IActionResult Enter()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
