using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.Models;
using BankingSystem.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.Controllers
{
    public class UsersController : Controller
    {
        private IUserManager _userManager;
        private IUsersRepository _repo;
        //UsersDataAccess users = new UsersDataAccess();

        public UsersController(IUserManager userManager, IUsersRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public IActionResult Register()
        {
            return View();
        }

        //Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind] Users usr)
        {
            if (ModelState.IsValid)
            {
                usr.AccountNumber = GenerateAccountNumber(15);
                usr.Balance = 0;
                usr.CreatedDate = DateTime.Now;
                _repo.AddUser(usr);
                return RedirectToAction("Index");
            }
            return View(usr);
        }

        //Login
        public IActionResult Login(string returnUrl)
        {

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        public IActionResult Index(string returnUrl)
        {

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Login Name/Password does not match.");
                return View(loginViewModel);
            }

            var user = _userManager.Validate(loginViewModel.LoginName, loginViewModel.Password);

            if (user.AccountNumber != null)
            {
                _userManager.SignIn(this.HttpContext, user, false);
                if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                    return RedirectToAction("Index", "UserTransactions", new { AccountNumber = user.AccountNumber });

                return Redirect(loginViewModel.ReturnUrl);
            }

            ModelState.AddModelError("Error", "User name/password not found.");
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _userManager.SignOut(this.HttpContext);
            return this.RedirectToAction("Login", "Users");
        }

        //CHEck account no if existing
        public bool CheckAccount(string AccountNumber)
        {
            var userTrans = _repo.GetUserByAccountNumber(AccountNumber);
            if (userTrans.AccountNumber != null)
                return true;
            return false;
        }

        //CHEck username if existing
        public bool CheckLogInName(string LoginName)
        {
            if (LoginName == null)
                return false;
            var userTrans = _repo.CheckLoginName(LoginName);
            if (userTrans)
                return true;
            return false;
        }

        // GENERATE ACCOUNT NUMBER
        public static string GenerateAccountNumber(int length)
        {
            var chars = "0123456789";
            var stringChars = new char[length];
            var random = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
