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

        public UserTransactionsController(IUserManager userManager, IUsersRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public IActionResult Index(string AccountNumber, int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var accntNumber = _repo.GetUserByAccountNumber(AccountNumber);
                var currentUser = _userManager.GetCurrentUser(this.HttpContext).AccountNumber;
                if (accntNumber.AccountNumber == null)
                {
                    return StatusCode(404);
                }
                else
                {
                    var item = _repo.GetUserTransactionsByAccountNumber(AccountNumber);

                    var recordCount = item.Count();
                    var noOfRecordsPerPage = 10.0;
                    var pageNumber = Math.Ceiling(recordCount / noOfRecordsPerPage);

                    if (currentUser != AccountNumber || currentUser == null)
                    {
                        return StatusCode(403);
                    }
                    if (page > pageNumber)
                    {
                        return StatusCode(404);
                    }
                    var userTrans = PagingList.Create(item, (int)noOfRecordsPerPage, page);

                    if (userTrans.Count == 1)
                        ModelState.AddModelError("Info", "No Current Record Found.");

                    userTrans.RouteValue = new RouteValueDictionary {
                { "AccountNumber", AccountNumber}
                };

                    return View(userTrans);
                }
            }
            else
            {
                return RedirectToAction("Login","Users");
            }
            
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

        public bool CheckBalance(string AccountNumber, double CurrBalance)
        {
            var result = _repo.CheckBalance(AccountNumber, CurrBalance);
            return result;
        }
    }
}
