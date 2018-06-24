using System;
using System.Linq;
using BankingSystem.Models;
using BankingSystem.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ReflectionIT.Mvc.Paging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.Controllers
{
    [Authorize]
    public class UserTransactionsController : Controller
    {
        private IUserManager _userManager;
        private IUsersRepository _repo;

        public UserTransactionsController(IUserManager userManager, IUsersRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public IActionResult Index(string ID, int page = 1)
        {
            var user = _repo.GetUserByID(ID);
            var currentUser = _userManager.GetCurrentUser(this.HttpContext).ID.ToString();
            if (user.ID == 0)
            {
                return StatusCode(404);
            }
            
            var item = _repo.GetUserTransactionsByID(ID);

            var recordCount = item.Count();
            var noOfRecordsPerPage = 10.0;
            var pageNumber = Math.Ceiling(recordCount / noOfRecordsPerPage);

            if (currentUser != ID)
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

            userTrans.RouteValue = new RouteValueDictionary { { "ID", ID } };

            return View(userTrans);
            
        }


        ////Insert Users Transaction
        //[HttpPost]
        //public IActionResult InsertTrans(UserTransactions usrTrans)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        usrTrans.TransDate = DateTime.Now;
        //        _repo.InsertUserTransactions(usrTrans);
        //    }
        //    return Ok(usrTrans);
        //}

        public bool CheckBalance(string AccountNumber, double CurrBalance)
        {
            var result = _repo.CheckBalance(AccountNumber, CurrBalance);
            return result;
        }

        public IActionResult Transact(string ID, string type)
        {
            string transType = "";
            if (type == "deposit")
            { transType = "Deposit"; }
            else if (type == "withdrawal")
            { transType = "Withdrawal"; }
            else if (type == "fundtransfer")
            { transType = "Fund Transfer"; }
            else
            {
                transType = "";
                    return StatusCode(404); }
            var model = new UserTransactions
            {
                ID = _userManager.GetCurrentUser(this.HttpContext).ID,
                Balance = _userManager.GetCurrentUser(this.HttpContext).Balance,
                TransType = transType
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transact([Bind] UserTransactions usrTrans, [FromQuery(Name = "type")] string type)
        {
            if (type == "deposit")
            { usrTrans.TransType = "Deposit"; }
            else if (type == "withdrawal")
            { usrTrans.TransType = "Withdrawal"; }
            else if (type == "fundtransfer")
            { usrTrans.TransType = "Fund Transfer"; }
            else
            {
                usrTrans.TransType = "";
                return StatusCode(404);
            }
            usrTrans.AccountNumber = _userManager.GetCurrentUser(this.HttpContext).AccountNumber;
            usrTrans.TransBy = _userManager.GetCurrentUser(this.HttpContext).AccountNumber;
            //usrTrans.TransType = "Deposit";
            usrTrans.TransDate = DateTime.Now;
            usrTrans.ID = _userManager.GetCurrentUser(this.HttpContext).ID;
            usrTrans.Balance = _userManager.GetCurrentUser(this.HttpContext).Balance;

            if (ModelState.IsValid)
            {
                _repo.InsertUserTransactions(usrTrans);
                return RedirectToAction("Index", "UserTransactions", new { id = usrTrans.ID });
            }
            return View(usrTrans);
        }

    }
}
