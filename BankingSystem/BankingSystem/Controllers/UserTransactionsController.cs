using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Models;
using BankingSystem.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ReflectionIT.Mvc;
using ReflectionIT.Mvc.Paging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.Controllers
{
    public class UserTransactionsController : Controller
    {
        private IUserManager _userManager;
        private IUsersRepository _repo;
        //UsersDataAccess users = new UsersDataAccess();

        public UserTransactionsController(IUserManager userManager, IUsersRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public IActionResult Index(string AccountNumber, int page=1)   
        {
            //if (this.User.Identity.IsAuthenticated)
            //{
                var item = _repo.GetUserTransactionsByAccountNumber(AccountNumber);
                //var userTrans = users.GetUserTransactionsByAccountNumber(AccountNumber);
                var userTrans = PagingList.Create(item, 10, page);
                if (userTrans.Count == 1)
                    ModelState.AddModelError("Info", "No Current Record Found.");

                userTrans.RouteValue = new RouteValueDictionary {
                { "AccountNumber", AccountNumber}
                };

                return View(userTrans);
            //}
            //else
            //{
            //    return this.RedirectToAction("Login", "Users");
            //}
           
        }


        //Insert Users Transaction
        [HttpPost]
        public IActionResult InsertTrans(UserTransactions usrTrans)
        {
            if (ModelState.IsValid)
            {
                usrTrans.TransDate = DateTime.Now;
                _repo.InsertUserTransactions(usrTrans);
            }
            return Ok(usrTrans);
        }
    }
}
