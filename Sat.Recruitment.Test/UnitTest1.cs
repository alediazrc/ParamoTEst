using System;
using System.Dynamic;

using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Services;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        private UserService userService = new UserService();
           
        [Fact]
        public void Test1()
        {
            
            User testUser = new User();
            testUser.Name = "Mike";
            testUser.Email = "mike@gmail.com";
            testUser.Address = "Av. Juan G";
            testUser.Phone = "+349 1122354215";
            testUser.UserType = "Normal";
            testUser.Money = decimal.Parse("124");

            var result = userService.CreateUser(testUser).Result;


            Assert.Equal(true, result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void Test2()
        {
            User testUser = new User();
            testUser.Name = "Agustina";
            testUser.Email = "Agustina@gmail.com";
            testUser.Address = "Av. Juan G";
            testUser.Phone = "+349 1122354215";
            testUser.UserType = "Normal";
            testUser.Money = decimal.Parse("124");

            var result = userService.CreateUser(testUser).Result;

            Assert.Equal(false, result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }
    }
}
