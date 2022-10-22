using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using ProjectEtoPrototype.Data;
using ProjectEtoPrototype.Models;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectEtoPrototype.Controllers
{
    /* 
     TODO: 
    المصروفات 
    المستخدم يضغط زائد 
    يحط الاسم (يطلع له اخر الاسماء)
    يحط الوصف
    يحط المبلغ 
    يختار دخل او مصروف
    فيه زر يوريك الملخص والاحصائيات
     */
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
            Db.SaveChanges();
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
            Db.SaveChanges();

            if (user.Preference.SurahId == 1 && user.Preference.VerseId == 1)
            {
                TempData["QuranComplete"] = "true";
            }

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
            Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult RemoveDailyTask(int taskId)
        {
            DailyTask dailyTask = Db.DailyTasks.Find(taskId);
            Db.DailyTasks.Remove(dailyTask);

            Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DelayDailyTask(int taskId)
        {
            DailyTask dailyTask = Db.DailyTasks.Find(taskId);
            dailyTask.CreatedDate = dailyTask.CreatedDate.AddHours(2);

            Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult AddCalories(int amount)
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            user.Preference.CurrentCalories += amount;
            Db.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}