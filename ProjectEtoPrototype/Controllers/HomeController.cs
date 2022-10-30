using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        if (CheckUserExist(Request) != null) return CheckUserExist(Request);
        var user = GetUser(Request);

        QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);

        foreach (var dailyTask in user.DailyTasks.Where(obj => 24 - (DateTime.Now - obj.CreatedDate).TotalHours <= 0))
            Db.DailyTasks.Remove(dailyTask);
        Db.SaveChanges();

        if ((DateTime.Now - user.Preference.CaloriesLstDateTime).TotalHours >= 24)
        {
            user.Preference.CaloriesLstDateTime = DateTime.Now;
            user.Preference.CurrentCalories = user.Preference.MaxCalories;
            Db.SaveChanges();
        }

        return View(user);
    }

    public IActionResult PreVerse()
    {
        if (CheckUserExist(Request) != null) return CheckUserExist(Request);
        var user = GetUser(Request);

        QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);
        QuranHandler.PreviousVerse();
        user.Preference.SurahId = QuranHandler.ChapterID;
        user.Preference.VerseId = QuranHandler.VerseID;
        Db.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult NextVerse()
    {
        if (CheckUserExist(Request) != null) return CheckUserExist(Request);
        var user = GetUser(Request);

        QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);
        QuranHandler.NextVerse();
        user.Preference.SurahId = QuranHandler.ChapterID;
        user.Preference.VerseId = QuranHandler.VerseID;
        Db.SaveChanges();

        if (user.Preference.SurahId == 1 && user.Preference.VerseId == 1) TempData["QuranComplete"] = "true";

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddDailyTask(User passedUser)
    {
        if (string.IsNullOrEmpty(passedUser.TempData))
        {
            TempData["AddDailyTaskError"] = "يجب ادخال الاسم";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        if (CheckUserExist(Request) != null) return CheckUserExist(Request);
        var user = GetUser(Request);

        user.DailyTasks.Add(new DailyTask { Name = passedUser.TempData });
        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult RemoveDailyTask(int taskId)
    {
        var dailyTask = Db.DailyTasks.Find(taskId);
        Db.DailyTasks.Remove(dailyTask);

        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult DelayDailyTask(int taskId)
    {
        var dailyTask = Db.DailyTasks.Find(taskId);
        dailyTask.CreatedDate = dailyTask.CreatedDate.AddHours(2);

        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    // TODO: add and remove calories from frontend without reloading page
    public IActionResult AddCalories(int amount)
    {
        if (CheckUserExist(Request) != null) return CheckUserExist(Request);
        var user = GetUser(Request);

        user.Preference.CurrentCalories += amount;
        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public void AddCaloriesApi(string userId, int amount)
    {
        User? user = Db.Users
            .Include(u => u.Preference)
            .First(u => u.UserId == userId);
        
        if (user == null) return;

        user.Preference.CurrentCalories += amount;
        Db.SaveChanges();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}