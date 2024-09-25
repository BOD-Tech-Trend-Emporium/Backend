using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
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
    }
}
