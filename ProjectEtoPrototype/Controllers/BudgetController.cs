using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEtoPrototype.Models;

namespace ProjectEtoPrototype.Controllers
{
    public class BudgetController : BaseController
    {
        public IActionResult Index()
        {
            // exist is a page that will be null if the user does exist
            var exist = CheckUserExist(Request);
            if (exist is not null) { return exist; }
            User user = GetUser(Request);

            return View(user);
        }

        // GET
        public IActionResult CreateOperation()
        {
            // exist is a page that will be null if the user does exist
            var exist = CheckUserExist(Request);
            if (exist is not null) { return exist; }
            User user = GetUser(Request);

            Operation oper = new Operation
            {
                // getting all the classes used
                lastClasses = 
                    (from obj in user.Bank.Operations
                    select obj.Class).Distinct().ToList()
            };

            return View(oper);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOperation(Operation operation)
        {

            User user = GetUser(Request);

            operation.Bank = user.Bank;
            
            if (!ModelState.IsValid || operation.Amount == 0)
            {
                operation.lastClasses =
                    (from obj in Db.Operations
                        where obj.Bank == user.Bank
                        select obj.Class).ToList();

                // this will display a message in frontend
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
            // exist is a page that will be null if the user does exist
            var exist = CheckUserExist(Request);
            if (exist is not null) { return exist; }
            User user = GetUser(Request);

            Operation? operation = Db.Operations.Find(operationId);
            operation!.lastClasses =
                (from obj in Db.Operations
                    where obj.Bank == user.Bank
                    select obj.Class).Distinct().ToList();
            return View(operation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOperation(Operation operation)
        {

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

            Operation? saveOperation = user.Bank.Operations.Find(x => x.OperationId == operation.OperationId);
            operation.CreatedDate = saveOperation!.CreatedDate;
            user.Bank.Operations.Remove(saveOperation);

            user.Bank.Operations.Add(operation);

            Db.SaveChanges();


            return RedirectToAction("Index", "Budget");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveOperation(int operationId)
        {

            User user = GetUser(Request);

            Operation? operation = Db.Operations.Find(operationId);

            user.Bank.Balance -= operation!.Amount;

            Db.Remove(operation);
            Db.SaveChanges();

            return RedirectToAction("Index", "Budget");
        }

        public IActionResult Reset()
        {
            // exist is a page that will be null if the user does exist
            var exist = CheckUserExist(Request);
            if (exist is not null) { return exist; }
            User user = GetUser(Request);

            Db.Operations.RemoveRange(user.Bank.Operations);

            user.Bank.Balance = 0;

            Db.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
