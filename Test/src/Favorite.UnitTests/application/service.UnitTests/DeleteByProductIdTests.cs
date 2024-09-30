using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Favorite.application.mappers;
using Api.src.Favorite.application.service;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.entity;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Test.src.Favorite.UnitTests.application.service.UnitTests
{
    public class DeleteByProductIdTests
    
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
        public async void Given_NonExistingFavorite_When_DeleteFavorite_Then_NonFoundException() 
        {
            var dbContext = await GetDataBaseContext();
            Guid userId = new Guid();
            Guid productId = new Guid();

            DeleteByProductId deleteByProductId = new(dbContext);

            Func<Task> act = () => deleteByProductId.Run(productId, userId);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Favorite product with id {productId} not found in favorites for {userId}");
        }

        [Fact]
        public async void Given_ExistingFavorite_When_DeleteFavorite_Then_DeletedFavoriteDto() 
        {
            var dbContext = await GetDataBaseContext();
            Guid userId = new Guid();
            Guid productId = new Guid();
            Guid categoryId = new Guid();

            CategoryEntity category = new() { 
                Id = categoryId,
                Name = "Books", 
                Status = CategoryStatus.Created 
            };
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();
            
            ProductEntity product = new()
            {
                Id = productId,
                Category = category,
                Title = "Title",
                Price = 10,
                Description = "Description",
                Image = "Image",
                Stock = 10,
                Status = ProductStatus.Created
            };
            dbContext.Product.Add(product);
            await dbContext.SaveChangesAsync();

            UserEntity user = new()
            {   
                Id = userId,
                Name = "user",
                Email = "user",
                UserName = "user",
                Password = "user",
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
            };
            dbContext.User.Add(user);
            await dbContext.SaveChangesAsync();

            FavoriteEntity favorite = new()
            {
                ProductId = product.Id,
                UserId = user.Id
            };
            dbContext.Favorite.Add(favorite);
            await dbContext.SaveChangesAsync();

            DeleteByProductId deleteByProductId = new(dbContext);

            DeletedFavoriteDto? result = await deleteByProductId.Run(product.Id, user.Id);

            result!.UserId.Should().Be(user.Id);
            result!.ProductId.Should().Be(product.Id);
        }
    }
}