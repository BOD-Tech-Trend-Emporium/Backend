using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;

namespace Test.User.UnitTests.application.UnitTests.service.UnitTests
{
    public class GetAllUsersTests
    {

        [Fact]
        public async void Given_AllUsers_When_AllUsersWithCreatedStatusAndNotAdminRole_Then_UsersListTask()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            await dbContext.SaveChangesAsync();

            GetAllUsers getAllUsers = new GetAllUsers(dbContext);

            //ACT
            var result = await getAllUsers.Run();
            var expected = new List<UserEntity>() { user, user2 };
            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expected);

        }
        [Fact]
        public async void Given_AllUsers_When_AllUsersAreAdmin_Then_EmptyListTask()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Admin, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Admin, Status = UserStatus.Created };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            await dbContext.SaveChangesAsync();

            GetAllUsers getAllUsers = new GetAllUsers(dbContext);

            //ACT
            var result = await getAllUsers.Run();
            //Assert
            result.Should().HaveCount(0);
            result.Should().BeEmpty();
        }

        [Fact]
        public async void Given_AllUsers_When_UsersHaveCreatedStatusAndRemovedStatusAndOneAdmin_Then_UsersListTask()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };
            UserEntity user3 = new() { Name = "Fizz", Email = "Fizz@gmail.com", UserName = "XxFizzxX", Password = "fizzTheBest", Role = UserRole.Admin, Status = UserStatus.Created };
            UserEntity user4 = new() { Name = "Yone", Email = "Yone@gmail.com", UserName = "YoneGg", Password = "Yauso's brother", Role = UserRole.Shopper, Status = UserStatus.Removed };
            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            dbContext.User.Add(user3);
            dbContext.User.Add(user4);
            await dbContext.SaveChangesAsync();

            GetAllUsers getAllUsers = new GetAllUsers(dbContext);

            //ACT
            var result = await getAllUsers.Run();
            var expected = new List<UserEntity>() { user, user2 };
            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expected);

        }
    }
}
