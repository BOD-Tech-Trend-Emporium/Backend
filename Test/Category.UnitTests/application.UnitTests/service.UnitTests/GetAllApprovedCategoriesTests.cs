using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using backend.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Test.Category.UnitTests.application.UnitTests.service.UnitTests
{
    public class GetAllApprovedCategoriesTests
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
        public async void Given_AllCategories_When_CategoriesWithNoCreatedStatus_Then_EmptyListTask()
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            GetAllApprovedCategories getAllApprovedCategories = new GetAllApprovedCategories(dbContext);
            CategoryEntity category = new() { Name = "Books", Status = CategoryStatus.Removed };
            CategoryEntity category2 = new() { Name = "Videogames", Status = CategoryStatus.Removed };

            dbContext.Category.Add(category);
            dbContext.Category.Add(category2);
            await dbContext.SaveChangesAsync();

            //ACT
            var result = await getAllApprovedCategories.Run();

            //Assert
            result.Should().BeEmpty();

        }

        [Fact]
        public async void Given_AllCategories_When_CategoriesWithCreatedStatusAndOtherStatus_Then_ApprovedCategoriesListTask()
        {
            //Arrange
            var dbContext = await GetDataBaseContext();
            GetAllApprovedCategories getAllApprovedCategories = new GetAllApprovedCategories(dbContext);
            CategoryEntity category = new() { Name = "Books", Status = CategoryStatus.Removed };
            CategoryEntity category2 = new() { Name = "Videogames", Status = CategoryStatus.ToCreate };
            CategoryEntity category3 = new() { Name = "Pictures", Status = CategoryStatus.Created };
            CategoryEntity category4 = new() { Name = "Music", Status = CategoryStatus.Created };
            CategoryEntity category5 = new() { Name = "League Of Legends", Status = CategoryStatus.ToRemove };


            dbContext.Category.Add(category);
            dbContext.Category.Add(category2);
            dbContext.Category.Add(category3);
            dbContext.Category.Add(category4);
            dbContext.Category.Add(category5);
            await dbContext.SaveChangesAsync();

            //ACT
            var result = await getAllApprovedCategories.Run();
            var expected = new List<CategoryEntity>() { category3, category4 };

            //Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expected);

        }
    }
}
