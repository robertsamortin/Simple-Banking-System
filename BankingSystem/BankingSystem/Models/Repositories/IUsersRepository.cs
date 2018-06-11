using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models.Repositories
{
    public interface IUsersRepository
    {
        void AddUser(Users usr);
        Users UserLogin(string LoginName, string Password);
        Users GetUserByAccountNumber(string AccountNumber);
        bool CheckLoginName(string LoginName);
        List<UserTransactions> GetUserTransactionsByAccountNumber(string AccountNumber);
        void InsertUserTransactions(UserTransactions usrTrans);
    }
}
