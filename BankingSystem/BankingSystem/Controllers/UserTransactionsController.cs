using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.Controllers
{
    public class UserTransactionsController : Controller
    {
        private IUserManager _userManager;
        UsersDataAccess users = new UsersDataAccess();

        public UserTransactionsController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index(string AccountNumber)   
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userTrans = users.GetUserTransactionsByAccountNumber(AccountNumber);
                if (userTrans.Count == 0)
                    ModelState.AddModelError("Info", "No Current Record Found.");
                return View(userTrans);
            }
            else
            {
                return this.RedirectToAction("Login", "Users");
            }
           
        }


        //Insert Users Transaction
        [HttpPost]
        public IActionResult InsertTrans(UserTransactions usrTrans)
        {
            if (ModelState.IsValid)
            {
                usrTrans.TransDate = DateTime.Now;
                users.InsertUserTransactions(usrTrans);
            }
            return Ok(usrTrans);
        }
    }
}
