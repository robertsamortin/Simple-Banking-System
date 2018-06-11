using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models.Repositories
{
    public interface IUserManager
    {
        Users Validate(string LoginName, string Password);
        void SignIn(HttpContext httpContext, Users user, bool isPersistent = false);
        void SignOut(HttpContext httpContext);
        string GetCurrentUserId(HttpContext httpContext);
        Users GetCurrentUser(HttpContext httpContext);
    }
}
