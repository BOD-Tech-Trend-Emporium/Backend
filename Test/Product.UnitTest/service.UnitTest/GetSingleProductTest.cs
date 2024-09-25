using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Product.application.service;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Common;

namespace Test.Product.UnitTest.service.UnitTest
{
    public class GetSingleProductTest
    {
        [Fact]
        public async void Given_GetsingleProduct_When_ProductsExists_Then_Product()
        {
            // Create new Category
            var dbContext = Utils.GetDataBaseContext();
            Guid categoryId = Guid.NewGuid();
            var categoryName = "Books";
            CategoryEntity category = new()
            {
                Id = categoryId,
                Name = categoryName,
                Status = CategoryStatus.Created
            };
            dbContext.Category.Add(category);

            // Create products with existing category
            var description = "Description";
            var image = "image";
            var title = "Title";
            var stock = 30;
            var prId = Guid.NewGuid();
            ProductEntity product = new()
            {
                Id = prId,
                Image = image,
                Title = title,
                Category = category,
                Stock = stock,
                Description = description,
                Status = Api.src.Product.domain.enums.ProductStatus.Created,
            };
            dbContext.Product.Add(product);

            await dbContext.SaveChangesAsync();
            GetProductById getAllProducts = new(dbContext);

            // ACT
            ProductByIdDto act = await getAllProducts.Run(prId);

            // Assert
            act.Id.Should().Be(prId);
            act.Image.Should().Be(image);
            act.Title.Should().Be(title);
            act.Category.Should().Be(categoryName);
        }

        [Fact]
        public async void Given_GetsingleProduct_When_ProductNOTExists_Then_ThrowNotFoundException()
        {
            // Create new Category
            var dbContext = Utils.GetDataBaseContext();

            var nonExistingProduct = Guid.NewGuid();
            
            GetProductById getAllProducts = new(dbContext);

            // ACT
            Func<Task> act = ()=> getAllProducts.Run(nonExistingProduct);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage($"product with id {nonExistingProduct} not found");
        }

        [Fact]
        public async void Given_GetsingleProduct_When_ProductNOTAvailable_Then_ThrowNotFoundException()
        {
            // Create new Category
            var dbContext = Utils.GetDataBaseContext();

            Guid categoryId = Guid.NewGuid();
            var categoryName = "Books";
            CategoryEntity category = new()
            {
                Id = categoryId,
                Name = categoryName,
                Status = CategoryStatus.Created
            };
            dbContext.Category.Add(category);

            // Create products with existing category
            var description = "Description";
            var image = "image";
            var title = "Title";
            var stock = 30;
            var prId = Guid.NewGuid();
            ProductEntity product = new()
            {
                Id = prId,
                Image = image,
                Title = title,
                Category = category,
                Stock = stock,
                Description = description,
                Status = Api.src.Product.domain.enums.ProductStatus.Removed,
            };
            dbContext.Product.Add(product);

            await dbContext.SaveChangesAsync();

            GetProductById getAllProducts = new(dbContext);

            // ACT
            Func<Task> act = () => getAllProducts.Run(prId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage($"product with id {prId} not found");
        }
    }
}
