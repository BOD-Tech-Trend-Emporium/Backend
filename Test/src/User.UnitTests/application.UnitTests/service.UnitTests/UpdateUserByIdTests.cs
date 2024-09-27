using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.service;
using Api.src.Auth.domain.dto;
using Api.src.Common.exceptions;
using Api.src.Session.domain.entity;
using Api.src.User.domain.dto;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Test.src.User.UnitTests.application.UnitTests.service.UnitTests
{
    public class UpdateUserByIdTests
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
        public async void Given_NonExistingUserId_When_UserTriesToUpdate_Then_NotFoundException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            Guid id = new();
            UpdateUserDto user = new();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            Func<Task> result = () => updateUserById.Run(id, user);

            //Assert
            await result.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with id {id} not found");
        }

        [Fact]
        public async void Given_ExistingUserWithNameToChange_When_UserTriesToUpdate_Then_NameUpdated() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity user = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                Name = "userUpdated"
            };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            UpdateUserResultDto result = await updateUserById.Run(user.Id, updateUser);

            //Assert
            result.Name.Should().Be(updateUser.Name);
        }

        [Fact]
        public async void Given_ExistingUserWithEmailToChange_When_UserTriesToUpdate_Then_EmailUpdated() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity user = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                Email = "user_pdated@gmail.com"
            };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            UpdateUserResultDto result = await updateUserById.Run(user.Id, updateUser);

            //Assert
            result.Email.Should().Be(updateUser.Email);
        }

        [Fact]
        public async void Given_ExistingUserWithUserNameToChange_When_UserTriesToUpdate_Then_UserNameUpdated() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity user = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                UserName = "userUpdated"
            };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            UpdateUserResultDto result = await updateUserById.Run(user.Id, updateUser);

            //Assert
            result.UserName.Should().Be(updateUser.UserName);
        }

        [Fact]
        public async void Given_ExistingUserWithPasswordToChange_When_UserTriesToUpdate_Then_PasswordUpdated() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            var configuration = await GetConfiguration();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity user = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                Password = "userPasswordUpdated"
            };
            UserLoginDto userToLogin = new() { Email = "user@gmail.com", Password = updateUser.Password};
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            SessionEntity session = new(){UserId = user.Id, IsActive = false};
            dbContext.Session.Add(session);

            UpdateUserById updateUserById = new(dbContext);
            LoginUser loginUser = new(dbContext, configuration);

            //Act
            await updateUserById.Run(user.Id, updateUser);
            LoggedUserDto? result = await loginUser.Run(userToLogin);

            //Assert
            result!.Email.Should().Be(user.Email);
        }
    
        [Fact]
        public async void Given_ExistingUserWithTakenEmailToChange_When_UserTriesToUpdate_Then_ConflictException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity oldUser = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UserEntity user = new(){
                Name = "user",
                Email = "user1@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                Email = "user@gmail.com"
            };
            dbContext.User.Add(oldUser);
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            Func<Task> result = () => updateUserById.Run(user.Id, updateUser);

            //Assert
            await result.Should().ThrowAsync<ConflictException>()
            .WithMessage("A user with this email alreay exists");
        }
        
        [Fact]
        public async void Given_ExistingUserWithInvalidEmailToChange_When_UserTriesToUpdate_Then_BadRequestException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity user = new(){
                Name = "user",
                Email = "user1@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                Email = "invalidEmail"
            };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            Func<Task> result = () => updateUserById.Run(user.Id, updateUser);

            //Assert
            await result.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Email not valid");
        }

        [Fact]
        public async void Given_ExistingUserWithTakenUserNameToChange_When_UserTriesToUpdate_Then_ConflictException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity oldUser = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UserEntity user = new(){
                Name = "user",
                Email = "user1@gmail.com",
                UserName = "user1",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            UpdateUserDto updateUser = new(){
                UserName = "user"
            };
            dbContext.User.Add(oldUser);
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateUserById updateUserById = new(dbContext);

            //Act
            Func<Task> result = () => updateUserById.Run(user.Id, updateUser);

            //Assert
            await result.Should().ThrowAsync<ConflictException>()
            .WithMessage("A user with this username alreay exists");
        }
    
    }
}