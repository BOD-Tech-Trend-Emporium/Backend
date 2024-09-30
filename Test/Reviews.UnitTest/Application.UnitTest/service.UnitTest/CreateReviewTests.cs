using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using Api.src.Review.application.service;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Reviews.UnitTest.Application.UnitTest.service.UnitTest
{
    public class CreateReviewTests
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
        public async void GivenNewReview_When_NewReview_Then_ReviewEntity()
        {
            //configuring base 
            var dbContext = await GetDataBaseContext();

            // Create Category
            CategoryEntity category = new() { Name = "Books", Status = CategoryStatus.Created };
            UserRole role = UserRole.Employee;
            CategoryStatus categoryStatus = CategoryStatus.ToCreate;
            // Create Product
            var productId = Guid.NewGuid();
            ProductEntity product = new()
            {
                Category = category,
                Description = "asd",
                Image = "asd",
                Id = productId,
                Status = ProductStatus.Created,
                Stock = 4,
                Title = "Title",
            };
            // Create user
            var userId = Guid.NewGuid();
            var username = "username";
            UserEntity user = new()
            {
                Id = userId,
                Password = "asd",
                UserName = username,
                Name = "asd",
                Role = UserRole.Employee,
                Status = UserStatus.Created,
                Email = "asd"
            };
            dbContext.User.Add(user);
            dbContext.Product.Add(product);
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();
            var comment = "comment";
            var rating = 5.0f;
            // test service
            CreateReviewDto newReview = new CreateReviewDto
            {
                productId = productId,
                comment = comment,
                rating = rating
            };

            CreateReview createReview = new CreateReview(dbContext);

            //ACT
            ReviewEntity result = await createReview.Run(newReview, userId);

            //Assert
            result.Comment.Should().Be(comment);
            result.Rating.Should().Be(rating);
            result.Product.Id.Should().Be(productId);
            result.User.Id.Should().Be(userId);
        }

    }
}
