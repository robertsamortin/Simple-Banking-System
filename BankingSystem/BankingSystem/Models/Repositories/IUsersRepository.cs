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
        Users GetUserByID(string ID);
        bool CheckLoginName(string LoginName);
        List<UserTransactions> GetUserTransactionsByID(string ID);
        void InsertUserTransactions(UserTransactions usrTrans);
        bool CheckBalance(string AccountNumber, double CurrBalance);
    }
}
