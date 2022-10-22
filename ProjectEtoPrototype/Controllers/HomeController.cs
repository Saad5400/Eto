using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Models;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectEtoPrototype.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);

            return View(user);
        }

        public IActionResult PreVerse()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);
            QuranHandler.PreviousVerse();
            user.Preference.SurahId = QuranHandler.ChapterID;
            user.Preference.VerseId = QuranHandler.VerseID;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult NextVerse()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);
            QuranHandler.NextVerse();
            user.Preference.SurahId = QuranHandler.ChapterID;
            user.Preference.VerseId = QuranHandler.VerseID;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDailyTask(User passedUser)
        {
            if (String.IsNullOrEmpty(passedUser.TempData))
            {
                TempData["AddDailyTaskError"] = "يجب ادخال الاسم";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            user.DailyTasks.Add(new DailyTask { Name = passedUser.TempData });
            _db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult RemoveDailyTask(int taskId)
        {
            DailyTask dailyTask = _db.DailyTasks.Find(taskId);
            _db.DailyTasks.Remove(dailyTask);

            _db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DelayDailyTask(int taskId)
        {
            DailyTask dailyTask = _db.DailyTasks.Find(taskId);
            dailyTask.CreatedDate = dailyTask.CreatedDate.AddHours(2);

            _db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult ChangeCurrentCalories(int amount)
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            user.Preference.CurrentCalories += amount;
            _db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}