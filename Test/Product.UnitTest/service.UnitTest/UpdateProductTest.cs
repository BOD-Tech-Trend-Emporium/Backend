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
    public class UpdateProductTest
    {
        [Fact]
        public async void Given_ProductToUpdate_When_ProductsExists_and_CategoryExists_Then_UpdatedEntity()
        {
            // Create new Category
            var dbContext = Utils.GetDataBaseContext();
            Guid categoryId = Guid.NewGuid();
            var categoryName = "Books";
            CategoryEntity category1 = new()
            {
                Id = categoryId,
                Name = categoryName,
                Status = CategoryStatus.Created
            };
            Guid categoryId2 = Guid.NewGuid();
            var categoryName2 = "new";
            CategoryEntity category2 = new()
            {
                Id = categoryId2,
                Name = categoryName2,
                Status = CategoryStatus.Created
            };
            dbContext.Category.Add(category1);
            dbContext.Category.Add(category2);

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
                Category = category1,
                Stock = stock,
                Description = description,
                Status = Api.src.Product.domain.enums.ProductStatus.Created,
            };
            dbContext.Product.Add(product);

            await dbContext.SaveChangesAsync();

            CreateProductDto updatedDto = new()
            {
                Category = categoryId2,
                Description = "new",
                Image = "new",
                Title = "new",
                Price = 10f,
                Stock = 10,
            };

            UpdateProduct updateProduct = new(dbContext);
            // ACT
            ProductEntity act = await updateProduct.Run(updatedDto,prId);

            // Assert
            act.Id.Should().Be(prId);
            act.Category.Id.Should().Be(categoryId2);
            act.Description.Should().Be("new");
            act.Image.Should().Be("new");
            act.Title.Should().Be("new");
            act.Stock.Should().Be(10);
            act.Prices.Should().NotBeEmpty();
        }

        
    }
}
