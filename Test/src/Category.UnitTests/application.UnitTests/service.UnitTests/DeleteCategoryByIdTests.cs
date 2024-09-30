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
    public class DeleteCategoryByIdTests
    {
        [Fact]
        public async void Given_AdminUser_When_CategoryDoesNotExist_Then_ConflictException()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Removed };
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Admin, Status = UserStatus.Created };

            dbContext.Add(category);
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            DeleteCategoryById deleteCategory = new DeleteCategoryById(dbContext);

            //ACT
            Func<Task> act = () => deleteCategory.Run(category.Id, user.Role);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category with id {category.Id} not found");

        }
        [Fact]
        public async void Given_AdminUser_When_CategoryExists_Then_DeleteCategoryByIdResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Created };
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Admin, Status = UserStatus.Created };
            DeleteCategoryByIdResponseDto deleteCategoryByIdResponseDto = new DeleteCategoryByIdResponseDto() {Message= "Deleted successfuly" };
            dbContext.Add(category);
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            DeleteCategoryById deleteCategory = new DeleteCategoryById(dbContext);

            //ACT
            var result = await deleteCategory.Run(category.Id, user.Role);

            //Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(deleteCategoryByIdResponseDto.Message);
            result.Should().BeOfType<DeleteCategoryByIdResponseDto>();
            result.Should().BeEquivalentTo(deleteCategoryByIdResponseDto);

        }

        public async void Given_EmployeeUser_When_CategoryExists_Then_DeleteCategoryByIdResponseDto()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Created };
            UserEntity user = new() { Name = "Sir Isaac Netwon", Email = "Isaac@gmail.com", UserName = "Isaac21", Password = "contrasena", Role = UserRole.Employee, Status = UserStatus.Created };
            DeleteCategoryByIdResponseDto deleteCategoryByIdResponseDto = new DeleteCategoryByIdResponseDto() { Message = "Pending to delete" };
            dbContext.Add(category);
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            DeleteCategoryById deleteCategory = new DeleteCategoryById(dbContext);

            //ACT
            var result = await deleteCategory.Run(category.Id, user.Role);

            //Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(deleteCategoryByIdResponseDto.Message);
            result.Should().BeOfType<DeleteCategoryByIdResponseDto>();
            result.Should().BeEquivalentTo(deleteCategoryByIdResponseDto);

        }

    }
}
