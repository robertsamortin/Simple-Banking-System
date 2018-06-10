using BankingSystem.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using BankingSystem.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest
{
    
    public class UsersTests
    {
        //private Mock<UsersDataAccess> users;
        //private Mock<IUserManager> userManager;
        //private UsersController controller;

        //public UsersTests()
        //{
        //    users = new Mock<UsersDataAccess>();
        //    userManager = new Mock<IUserManager>();
        //    controller = new UsersController(userManager.Object);
        //}

        //[Fact]
        //public void CheckAccount_ReturnsNotFound_WhenAccountNumberDoesNotExist()
        //{
        //    // Arrange
        //    var mockArticle = new Users { LoginName = "Samortin" };
        //    controller.ModelState.AddModelError("Description", "This field is required");

        //    // Act
        //    var result =  controller.Register(mockArticle);

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    Assert.Equal(mockArticle, viewResult.ViewData.Model);
        //    users.Verify(repo => repo.AddUser(mockArticle), Times.Never());
        //}
    }
}
