using BankingSystem.Controllers;
using BankingSystem.Models;
using BankingSystem.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest
{
    public class ConcurrencyTest
    {
        [Fact]
        public void Users_ReturnsFalse_WhenBalancesIsnotEqualToDB()
        {
            // Arrange
            var controller = new UserTransactionsController(new UserManager(new UsersRepository()), new UsersRepository());
            var oldUser = new Users
            {
                AccountNumber = "123456789123456",
                Balance = 500
            };
            double CurrBalance = 1000;
            // Act
            var result = controller.CheckBalance(oldUser.AccountNumber, CurrBalance);
           
            // Assert

            var okResult = Assert.IsType<bool>(result);
            Assert.False(okResult, "false");
        }
    }
}
