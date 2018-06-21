using BankingSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingSystem.Models.Repositories
{
    public class UserManager : IUserManager
    {
        private IUsersRepository _repo;
        public UserManager(IUsersRepository repo)
        {
            _repo = repo;
        }

        public Users GetCurrentUser(HttpContext httpContext)
        {
            string currentUserId = this.GetCurrentUserId(httpContext).ToString();

            if (currentUserId == null)
                return null;

            return _repo.GetUserByID(currentUserId);
        }

        public string GetCurrentUserId(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            Claim claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;

            string currentUserId = claim.Value;
            return currentUserId;
        }

        public async void SignIn(HttpContext httpContext, Users user, bool isPersistent = false)
        {
            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = isPersistent }
            );
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public Users Validate(string LoginName, string Password)
        {
            var user = _repo.UserLogin(LoginName, Password);

            return user;
        }

        
        private IEnumerable<Claim> GetUserClaims(Users user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.GivenName, user.LoginName));
            claims.Add(new Claim(ClaimTypes.Name, user.AccountNumber.ToString()));
            return claims;
        }
    }
}
