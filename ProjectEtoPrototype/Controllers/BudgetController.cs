﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class BudgetController : BaseController
    {
        public IActionResult Index()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            return View(user);
        }

        // GET
        public IActionResult CreateOperation()
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            Operation oper = new Operation
            {
                lastClasses = 
                    (from obj in Db.Operations
                    where obj.Bank == user.Bank
                        select obj.Class).Distinct().ToList()
            };

            return View(oper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOperation(Operation operation)
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            operation.Bank = user.Bank;
            
            if (!ModelState.IsValid || operation.Amount == 0)
            {
                operation.lastClasses =
                    (from obj in Db.Operations
                        where obj.Bank == user.Bank
                        select obj.Class).ToList();
                TempData["OperationError"] = "true";
                return View(operation);
            }

            user.Bank.Operations.Add(operation);
            user.Bank.Balance += operation.Amount;

            Db.SaveChanges();

            return RedirectToAction("Index", "Budget");
        }

        // GET
        public IActionResult EditOperation(int operationId)
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            Operation operation = Db.Operations.Find(operationId);
            operation.lastClasses =
                (from obj in Db.Operations
                    where obj.Bank == user.Bank
                    select obj.Class).Distinct().ToList();
            return View(operation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOperation(Operation operation)
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);
            if (!ModelState.IsValid || operation.Amount == 0)
            {
                operation.lastClasses =
                    (from obj in Db.Operations
                        where obj.Bank == user.Bank
                        select obj.Class).ToList();
                TempData["OperationError"] = "true";
                return View(operation);
            }

            Operation saveOperation = user.Bank.Operations.Find(x => x.OperationId == operation.OperationId);
            operation.CreatedDate = saveOperation.CreatedDate;
            user.Bank.Operations.Remove(saveOperation);

            user.Bank.Operations.Add(operation);

            Db.SaveChanges();


            return RedirectToAction("Index", "Budget");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveOperation(int operationId)
        {
            if (CheckUserExist(Request) != null) { return CheckUserExist(Request); }
            User user = GetUser(Request);

            Operation operation = Db.Operations.Find(operationId);

            user.Bank.Balance -= operation.Amount;

            Db.Remove(operation);
            Db.SaveChanges();

            return RedirectToAction("Index", "Budget");
        }
    }
}
