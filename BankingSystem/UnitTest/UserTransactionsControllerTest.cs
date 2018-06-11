using BankingSystem.Controllers;
using BankingSystem.Models;
using BankingSystem.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest
{
    public class UserTransactionsControllerTest
    {
        [Fact]
        public void Index_ReturnsViewResult_WhenLoginSucceeded()
        {
            // Arrange
            var controller = new UserTransactionsController(new UserManager(new UsersRepository()), new UsersRepository());
            var newUser = new Users
            {
                AccountNumber = "123456789123456"
            };
           
           // Act
            var result = controller.Index("123456789123456");

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName));
        }

        [Fact]
        public void InsertTrans_ReturnsOkObjectResult_WhenSuccess()
        {
            // Arrange
            var controller = new UserTransactionsController(new UserManager(new UsersRepository()), new UsersRepository());
            var newUserTrans = new UserTransactions
            {
                AccountNumber = "123456789123456",
                Amount = 100,
                TransType = "Deposit",
                TransBy = "123456789123456"
            };

            // Act
            var result = controller.InsertTrans(newUserTrans);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
