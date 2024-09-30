using System;
using System.Collections.Generic;
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

namespace Test.src.Auth.UnitTests.application.UnitTests.service.UnitTests
{
    public class Logout
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

        [Fact]
        public async void Given_NotLoggedUser_When_UserTriesToLogout_Then_ConflictException()
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
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
            string userId = userModel!.Id.ToString();
            SessionEntity session = new(){UserId = userModel!.Id, IsActive = false};
            dbContext.Session.Add(session);
            await dbContext.SaveChangesAsync();

            LogoutUser logoutUser = new(dbContext);

            //Act
            Func<Task> result = () => logoutUser.Run(userId);

            //Assert
            await result.Should().ThrowAsync<ConflictException>()
            .WithMessage("User doesn't have an active session");
        }
    
        [Fact]
        public async void Given_LoggedUser_When_UserTriesToLogout_Then_SessionClosed()
        {//Arrange
            var dbContext = await GetDataBaseContext();
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
            string userId = userModel!.Id.ToString();
            SessionEntity session = new(){UserId = userModel!.Id, IsActive = true};
            dbContext.Session.Add(session);
            await dbContext.SaveChangesAsync();

            LogoutUser logoutUser = new(dbContext);

            //Act
            SessionEntity? result = await logoutUser.Run(userId);

            //Assert
            result!.IsActive.Should().Be(false);
        }   
    }
}