using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Session.domain.entity;
using Api.src.User.application.service;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Test.Common;
namespace Test.User.UnitTests.application.UnitTests.service.UnitTests
{
    public class DeleteUserByIdTests
    {
        [Fact]
        public async void Given_UserIdDoesNotExist_When_ThereAreUsersToDelete_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            await dbContext.SaveChangesAsync();

            DeleteUserById deleteUserById = new DeleteUserById(dbContext);

            //ACT
            var userToDelete = Guid.NewGuid();
            Func<Task> act = () => deleteUserById.Run(userToDelete);
            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with id {userToDelete} not found");

        }

        [Fact]
        public async void Given_UserId_When_TheUserHasARemovedStatus_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Removed };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            await dbContext.SaveChangesAsync();

            DeleteUserById deleteUserById = new DeleteUserById(dbContext);

            //ACT
            var userToDelete = user2.Id;
            Func<Task> act = () => deleteUserById.Run(userToDelete);
            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with id {userToDelete} not found");

        }

        [Fact]
        public async void Given_UserId_When_TheUsersHaveCreatedStatus_Then_UserEntityTask()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            SessionEntity sesion = new SessionEntity() { IsActive = true };
            UserEntity user2 = new() { Name = "Yasuo", Email = "Yauso@gmail.com", UserName = "XxYausoxX", Password = "ggjgskfns", Role = UserRole.Shopper, Status = UserStatus.Created,Session= sesion };

            dbContext.User.Add(user);
            dbContext.User.Add(user2);
            dbContext.Session.Add(sesion);


            await dbContext.SaveChangesAsync();
            DeleteUserById deleteUserById = new DeleteUserById(dbContext);

            //ACT
            var userToDelete = user2.Id;
            var expectedStatus = UserStatus.Removed;
            UserEntity result = await deleteUserById.Run(userToDelete);

            //Assert
            result.Should().Be(user2);
            result.Status.Should().Be(expectedStatus);



        }

    }
}
