using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
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

        [Fact]
        public async void GivenNewReview_When_NewUserDontExist_Then_ThrowError()
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
            var fakeUserId = Guid.NewGuid();
            //ACT
            Func<Task> act = () => createReview.Run(newReview,fakeUserId );

            //Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"user with Id {fakeUserId} or product with id {productId} doesn't exist");
        }

        [Fact]
        public async void GivenNewReview_When_ProductDontExist_Then_ThrowError()
        {
            //configuring base 
            var dbContext = await GetDataBaseContext();

            
            // Create Product
            var fakeProductId = Guid.NewGuid();

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

            await dbContext.SaveChangesAsync();
            var comment = "comment";
            var rating = 5.0f;
            // test service
            CreateReviewDto newReview = new CreateReviewDto
            {
                productId = fakeProductId,
                comment = comment,
                rating = rating
            };

            CreateReview createReview = new CreateReview(dbContext);
            //ACT
            Func<Task> act = () => createReview.Run(newReview, userId);

            //Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"user with Id {userId} or product with id {fakeProductId} doesn't exist");
        }

        [Fact]
        public async void GivenNewReview_When_ratingNosValid_Then_ThrowError()
        {
            //configuring base 
            var dbContext = await GetDataBaseContext();
            
            var comment = "comment";
            var rating = 10.0f;
            // test service
            CreateReviewDto newReview = new CreateReviewDto
            {
                productId = Guid.NewGuid(),
                comment = comment,
                rating = rating
            };

            CreateReview createReview = new CreateReview(dbContext);
            //ACT
            Func<Task> act = () => createReview.Run(newReview, Guid.NewGuid());

            //Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"rating must be between 0 and 5, given: {rating}");
        }
    }
}
