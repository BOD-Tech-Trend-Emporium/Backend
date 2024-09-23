using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace Test.Category.UnitTests.application.UnitTests.service.UnitTests
{
    public class CreateCategoryTests
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
        public async void CreateCategory_Run_ReturnCategory() {
            //Arrange
            var dbContext = await GetDataBaseContext();
            CategoryEntity category = new() { Name = "Books", Status = Api.src.Category.domain.enums.CategoryStatus.Created};
            UserRole role = UserRole.Admin;
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();

            CreateCategory createCategory = new CreateCategory(dbContext);

            //ACT
            //var result = await createCategory.Run(category, role);
            Func<Task> act = () => createCategory.Run(category, role);
           
            //Assert
            await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Category already exists");

        }
    }
}
