using Api.src.Category.application.service;
using Api.src.Category.domain.dto;
using Api.src.Category.domain.entity;
using Api.src.Common.exceptions;
using Api.src.User.domain.enums;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Common;

namespace Test.src.Category.UnitTests.application.UnitTests.service.UnitTests
{
    public class UpdateCategoryByIdTests
    {
        [Fact]
        public async void Given_AdminUser_When_CategoryDoesNotExist_Then_NotFoundException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity categoryToUpdate = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Removed };
            CategoryEntity newCategory = new() { Name = "Shoes"};
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Admin, Status = UserStatus.Removed };

            dbContext.Add(categoryToUpdate);
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateCategoryById updateCategoryById = new UpdateCategoryById(dbContext);

            //ACT
            Func<Task> act = () => updateCategoryById.Run(categoryToUpdate.Id, newCategory, user.Role);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category does not exist with id {categoryToUpdate.Id}");

        }

        [Fact]
        public async void Given_AdminUser_When_CategoryToUpdateIsRepeated_Then_ConflictException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity categoryToUpdate = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Created };
            CategoryEntity newCategory = new() { Name = "Books" };
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Admin, Status = UserStatus.Removed };

            dbContext.Add(categoryToUpdate);
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateCategoryById updateCategoryById = new UpdateCategoryById(dbContext);

            //ACT
            Func<Task> act = () => updateCategoryById.Run(categoryToUpdate.Id, newCategory, user.Role);

            //Assert
            await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Category already exists");

        }

        [Fact]
        public async void Given_AdminUser_When_CategoryToUpdateIsNotRepeated_Then_UpdateCategoryByIdResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity categoryToUpdate = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Created };
            CategoryEntity newCategory = new() { Name = "Cars" };
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Admin, Status = UserStatus.Removed };
            UpdateCategoryByIdResponseDto updateCategoryByIdResponseDto = new UpdateCategoryByIdResponseDto() { Message = "Updated successfuly" };
            dbContext.Add(categoryToUpdate);
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            UpdateCategoryById updateCategoryById = new UpdateCategoryById(dbContext);

            //ACT
            var result = await updateCategoryById.Run(categoryToUpdate.Id, newCategory, user.Role);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateCategoryByIdResponseDto>();
            result.Should().BeEquivalentTo(updateCategoryByIdResponseDto);

        }
    }
}
