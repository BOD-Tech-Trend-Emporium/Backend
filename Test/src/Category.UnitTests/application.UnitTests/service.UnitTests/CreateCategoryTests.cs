using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Common;


namespace Test.Category.UnitTests.application.UnitTests.service.UnitTests
{
    public class CreateCategoryTests
    {
     
        [Fact]
        public async void Given_NewCategory_When_CategoryAlreadyCreated_Then_ConflictException() {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Created};
            UserRole role = UserRole.Admin;
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();

            CreateCategory createCategory = new CreateCategory(dbContext);

            //ACT
            Func<Task> act = () => createCategory.Run(category, role);
           
            //Assert
            await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Category already exists or is pending to be created");

        }

        [Fact]
        public async void Given_AdminCreatesANewCategory_When_CategoryDoesNotExist_Then_CategoryEntity()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = CategoryStatus.Created };
            UserRole role = UserRole.Admin;
            CategoryStatus categoryStatus = CategoryStatus.Created;
            CreateCategory createCategory = new CreateCategory(dbContext);

            //ACT

            CategoryEntity result = await createCategory.Run(category, role);

            //Assert
            result.Should().Be(category);
            result.Status.Should().Be(categoryStatus);

        }
        [Fact]
        public async void Given_EmployeeCreatesANewCategory_When_CategoryDoesNotExist_Then_CategoryEntity()
        {
            //Arrange
            var dbContext = Utils.GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = CategoryStatus.Created };
            UserRole role = UserRole.Employee;
            CategoryStatus categoryStatus = CategoryStatus.ToCreate;
            CreateCategory createCategory = new CreateCategory(dbContext);

            //ACT
            CategoryEntity result = await createCategory.Run(category, role);

            //Assert
            result.Should().Be(category);
            result.Status.Should().Be(categoryStatus);

        }
    }
}
