using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.service;
using Api.src.Common.exceptions;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Test.src.Auth.UnitTests.application.UnitTests.service.UnitTests
{
    public class SignUpShopperTests
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
        public async void Given_InvalidEmail_When_ShopperSignups_Then_BadRequestException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            UserEntity user = new(){
                Name = "user",
                Email = "user",
                UserName = "user",
                Password = "user",
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };

            SignUpShopper signUpShopper = new(dbContext);

            //ACT
            Func<Task> result = () => signUpShopper.Run(user);

            //Assert
            await result.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Email not valid");
        }

        [Fact]
        public async void Given_AlreadyExistingEmail_When_ShopperSignups_Then_ConflictException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity existingUser = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            dbContext.User.Add(existingUser);
            await dbContext.SaveChangesAsync();    

            UserEntity newUser = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = "user",
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };

            SignUpShopper signUpShopper = new(dbContext);

            //ACT
            Func<Task> result = () => signUpShopper.Run(newUser);

            //Assert
            await result.Should().ThrowAsync<ConflictException>()
            .WithMessage("A user with this email alreay exists");
        }
    
        [Fact]
        public async void Given_AlreadyExistingUsername_When_ShopperSignups_Then_ConflictException() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword("user");
            UserEntity existingUser = new(){
                Name = "user",
                Email = "user@gmail.com",
                UserName = "user",
                Password = passwordHash,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            dbContext.User.Add(existingUser);
            await dbContext.SaveChangesAsync();    

            UserEntity newUser = new(){
                Name = "user",
                Email = "user2@gmail.com",
                UserName = "user",
                Password = "user",
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };

            SignUpShopper signUpShopper = new(dbContext);

            //ACT
            Func<Task> result = () => signUpShopper.Run(newUser);

            //Assert
            await result.Should().ThrowAsync<ConflictException>()
            .WithMessage("A user with this username alreay exists");
        }
    }
}