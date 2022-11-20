using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        // exist is a page that will be null if the user does exist
        var exist = CheckUserExist(Request);
        if (exist is not null) { return exist; }
        var user = GetUser(Request);

        // ---- QuranHandler.SetAndGetVerse(user.Preference.SurahId, user.Preference.VerseId);

        // remove tasks that expired
        var changed = false;
        foreach (var dailyTask in user.DailyTasks.Where(obj => 24 - (DateTime.Now - obj.CreatedDate).TotalHours <= 0))
        { 
            Db.DailyTasks.Remove(dailyTask);
            changed = true;
        }

        // reset calories
        if ((DateTime.Now - user.Preference.CaloriesLstDateTime).TotalHours >= 24)
        {
            user.Preference.CaloriesLstDateTime = DateTime.Now;
            user.Preference.CurrentCalories = user.Preference.MaxCalories;
            changed = true;
        }

        // only save when something changed
        if (changed)
        {
            Db.SaveChanges();
        }

        return View(user);
    }

    public void UpdateUserQuranApi(string userId, int verseId)
    {
        var user = GetUser(userId);

        user.Preference.VerseId = verseId;
        Db.SaveChanges();
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

        var user = GetUser(Request);

        user.DailyTasks.Add(new DailyTask { Name = passedUser.TempData });
        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult RemoveDailyTask(int taskId)
    {
        var dailyTask = Db.DailyTasks.Find(taskId);
        Db.DailyTasks.Remove(dailyTask!);

        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult DelayDailyTask(int taskId)
    {
        var dailyTask = Db.DailyTasks.Find(taskId);
        dailyTask!.CreatedDate = dailyTask.CreatedDate.AddHours(2);

        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult AddCalories(int amount)
    {
        var user = GetUser(Request);

        user.Preference.CurrentCalories += amount;
        Db.SaveChanges();

        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpPost]
    public bool AddCaloriesApi(string userId, int amount)
    {
        var user = GetUser(userId);

        if (user.Preference.CurrentCalories + amount > 5000 || user.Preference.CurrentCalories + amount < -5000)
        {
            return false;
        }

        user.Preference.CurrentCalories += amount;
        Db.SaveChanges();

        return true;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}