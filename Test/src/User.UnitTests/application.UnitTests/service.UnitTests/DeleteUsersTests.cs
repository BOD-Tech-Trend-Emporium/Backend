using Api.src.Common.exceptions;
using Api.src.User.application.service;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;


namespace Test.User.UnitTests.application.UnitTests.service.UnitTests
{
    public class DeleteUsersTests
    {

        [Fact]
        public async void Given_AListOfEmails_When_NoEmailMatches_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            await dbContext.SaveChangesAsync();

            List<string> userEmails = new List<string>() { "Francis@gmail.com", "Pepe@gmail.com" };
            DeleteUsers deleteUsers = new DeleteUsers(dbContext);

            //ACT
            List<string> expectedNotFoundUsers = new List<string>() { "Francis@gmail.com", "Pepe@gmail.com" };
            Func<Task> act = () => deleteUsers.Run(userEmails);
            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Users/emails do not exist: "+ String.Join(" ", expectedNotFoundUsers));

        }

        [Fact]
        public async void Given_AEmpyEmails_When_NoEmailMatches_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            await dbContext.SaveChangesAsync();

            List<string> userEmails = new List<string>();
            DeleteUsers deleteUsers = new DeleteUsers(dbContext);

            //ACT
            List<string> expectedNotFoundUsers = new List<string>();
            Func<Task> act = () => deleteUsers.Run(userEmails);
            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Users/emails do not exist: " + String.Join(" ", expectedNotFoundUsers));

        }
        [Fact]
        public async void Given_AEmpyEmails_When_NoEmailExists_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();

            List<string> userEmails = new List<string>();
            DeleteUsers deleteUsers = new DeleteUsers(dbContext);

            //ACT
            List<string> expectedNotFoundUsers = new List<string>();
            Func<Task> act = () => deleteUsers.Run(userEmails);
            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Users/emails do not exist: " + String.Join(" ", expectedNotFoundUsers));

        }

        [Fact]
        public async void Given_AEmpyEmails_When_UsersWithCreatedStatusAndRemoveStatus_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };
            UserEntity user3 = new() { Name = "Yone", Email = "Yone@gmail.com", UserName = "Yone", Password = "Yone12343", Role = UserRole.Shopper, Status = UserStatus.Removed };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            dbContext.User.Add(user3);
            await dbContext.SaveChangesAsync();

            List<string> userEmails = new List<string>() { "Isaac@gmail.com", "Yauso@gmail.com", "Yone@gmail.com" };
            DeleteUsers deleteUsers = new DeleteUsers(dbContext);

            //ACT
            List<string> expectedNotFoundUsers = new List<string>() { "Yone@gmail.com" };
            Func<Task> act = () => deleteUsers.Run(userEmails);
            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Users/emails do not exist: " + String.Join(" ", expectedNotFoundUsers));

        }

        [Fact]
        public async void Given_UserEmails_When_UsersWithCreatedStatusAndRemoveStatus_Then_SeccessfullyRemove()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };
            UserEntity user3 = new() { Name = "Yone", Email = "Yone@gmail.com", UserName = "Yone", Password = "Yone12343", Role = UserRole.Shopper, Status = UserStatus.Created };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            dbContext.User.Add(user3);
            await dbContext.SaveChangesAsync();

            List<string> userEmails = new List<string>() { "Isaac@gmail.com"};
            DeleteUsers deleteUsers = new DeleteUsers(dbContext);

            //ACT
            var expectedRemoveStatus = UserStatus.Removed;
            var expectedCreatedStatus = UserStatus.Created;
            await deleteUsers.Run(userEmails);

            //Assert
            user.Status.Should().Be(expectedRemoveStatus);
            user2.Status.Should().Be(expectedCreatedStatus);
            user3.Status.Should().Be(expectedCreatedStatus);

        }
    }
}
