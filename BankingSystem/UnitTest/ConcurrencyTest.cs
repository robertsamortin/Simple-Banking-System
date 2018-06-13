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
        public void Users_ReturnsTrue_WhenBalancesIsnotEqualToDB()
        {
            // Arrange
            var controller = new UserTransactionsController(new UserManager(new UsersRepository()), new UsersRepository());
            var oldUser = new Users
            {
                AccountNumber = "123456789123456",
                Balance = 1000
            };

            var newUser = new Users
            {
                AccountNumber = "123456789123456",
                Balance = 5000
            };

            // Act
            var result = controller.CheckBalance(oldUser.AccountNumber);
            var result1 = controller.CheckBalance(oldUser.AccountNumber);
           
            // Assert

            var okResult = Assert.IsType<double>(result);
            var okResult1 = Assert.IsType<double>(result1);
            Assert.Equal(okResult, okResult1);
        }
    }
}
