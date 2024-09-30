using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.service;
using Api.src.Auth.domain.dto;
using Api.src.Common.exceptions;
using Api.src.Session.domain.entity;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Test.Auth.UnitTests.application.UnitTests.service.UnitTests
{
    public class LoginUserTests
    {
        private async Task<ApplicationDBContext> GetDataBaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDBContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        private async Task<IConfiguration> GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"AppSettings:TokenKey", "fakeTokenKeyfakeTokenKeyfakeTokenKeyfakeTokenKey"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return configuration;
        }

        [Fact]
        public async void Given_WrongEmailOrPassword_When_UserTriesToLogIn_Then_BadRequestException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            var configuration = await GetConfiguration();
            UserLoginDto user = new() { Email = "user@gmail.com", Password = "user"};
            LoginUser loginUser = new(dbContext, configuration);

            //Act
            Func<Task> result = () => loginUser.Run(user);

            //Assert
            await result.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Wrong email or password");
        }
    
        [Fact]
        public async void Given_UserToLogin_When_UserAlreadyLogged_Then_ConflictException()
         {
            //Arrange
            var dbContext = await GetDataBaseContext();
            var configuration = await GetConfiguration();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserLoginDto userToLogin = new() { Email = "user@gmail.com", Password = "user"};
            UserEntity user = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();
            var userModel = await dbContext.User.FirstOrDefaultAsync(x => x.Email == "user@gmail.com" && x.Status == UserStatus.Created);
            SessionEntity session = new(){UserId = userModel!.Id, IsActive = true};
            dbContext.Session.Add(session);
            await dbContext.SaveChangesAsync();

            LoginUser loginUser = new(dbContext, configuration);

            //Act
            Func<Task> result = () => loginUser.Run(userToLogin);

            //Assert
            await result.Should().ThrowAsync<ConflictException>()
            .WithMessage("User already logged in");
         }
    
        [Fact]
        public async void Given_UserToLogin_When_ValidParamsAndUserNotLoggedIn_Then_SuccessfulLogin()
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            var configuration = await GetConfiguration();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserLoginDto userToLogin = new() { Email = "user@gmail.com", Password = "user"};
            UserEntity user = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };

            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();
            var userModel = await dbContext.User.FirstOrDefaultAsync(x => x.Email == "user@gmail.com" && x.Status == UserStatus.Created);
            SessionEntity session = new(){UserId = userModel!.Id, IsActive = false};
            dbContext.Session.Add(session);
            await dbContext.SaveChangesAsync();

            LoginUser loginUser = new(dbContext, configuration);

            //Act
            LoggedUserDto? result = await loginUser.Run(userToLogin);

            //Assert
            result!.Email.Should().Be(user.Email);
        }
    }
}