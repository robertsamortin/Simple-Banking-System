using BankingSystem.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using BankingSystem.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingSystem;
using BankingSystem.Models.Repositories;
using FluentAssertions;

namespace UnitTest
{
    
    public class UsersControllerTests
    {
        
        [Fact]
        public void Register_ReturnsRedirect_WhenRegistrationIsSuccess()
        {
            // Arrange
            var controller = new UsersController(new UserManager(new UsersRepository()), new UsersRepository());
            var newUser = new Users
            {
                LoginName = "robert",
                Password = "robert123"
            };

            // Act
            var result =  controller.Register(newUser);

            // Assert
           
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Register_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new UsersController(new UserManager(new UsersRepository()), new UsersRepository());
            controller.ModelState.AddModelError("LoginName", "Required");
            var newUser = new Users
            {
                LoginName = "",
                Password = "robert123"
            };

            // Act
            var result = controller.Register(newUser);

            // Assert
            var badRequestResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(badRequestResult.ViewName));
        }
    }
}
