using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.dto;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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

        [Fact]
        public async void Given_NonExistingUserId_When_UserTriesToLogIn_Then_NotFoundException() 
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
    }
}