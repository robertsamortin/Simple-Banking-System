using BankingSystem.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using BankingSystem.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingSystem;

namespace UnitTest
{
    
    public class UsersControllerTests
    {
        
        [Fact]
        public void CheckAccount_ReturnsNotFound_WhenAccountNumberDoesNotExist()
        {
            // Arrange
            var controller = new UsersController(new UserManager());
            var newUser = new Users
            {
                AccountNumber = "123456789012345",
                LoginName = "robert",
                Password = "robert123",
                Balance = 0,
                CreatedDate = DateTime.Now
            };

            // Act
            var result =  controller.Register(newUser);

            // Assert
            var okResult = result.ToString();
            //var okResult = result.BeOfType<CreatedAtActionResult>().Subject;
            //var person = okResult.Value.Should().BeAssignableTo<Users>().Subject;
          //  person.Id.Should().Be(51);
        }
    }
}
