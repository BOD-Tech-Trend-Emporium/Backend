using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Product.application.service;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using backend.Data;
using backend.src.User.domain.enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Common;

namespace Test.Product.UnitTest.service.UnitTest
{
    public class CreateProductTest
    {
        private ApplicationDBContext GetDataBaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDBContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async void Given_NewProduct_When_CategoryNotExistOr_Then_NotFoundException()
        {
            // Create new Category
            var dbContext = Utils.GetDataBaseContext();
            Guid categoryId = Guid.NewGuid();
            CategoryEntity category = new() { 
                Id = categoryId,
                Name = "Books", 
                Status = CategoryStatus.Created 
            };
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();

            // Create product with no existing category
            Guid unexistingCategoryId = Guid.NewGuid();
            CreateProductDto product = new()
            {
                Category = unexistingCategoryId,
                Description = "Description",
                Image = "image",
                Price = 120.0f,
                Title = "Title",
                Stock = 30

            };

            CreateProduct createProduct = new(dbContext);


            // ACT
            Func<Task> act = () => createProduct.Run(product, UserRole.Admin);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{unexistingCategoryId}' not found");
        }

        [Fact]
        public async void Given_NewProduct_When_CategoryExists_Then_newProductEntity()
        {
            // Create new Category
            var dbContext = Utils.GetDataBaseContext();
            Guid categoryId = Guid.NewGuid();
            CategoryEntity category = new()
            {
                Id = categoryId,
                Name = "Books",
                Status = CategoryStatus.Created
            };
            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();

            // Create product with existing category
            var description = "Description";
            var image = "image";
            var title = "Title";
            var stock = 30;
            var price = 10.3f;
            CreateProductDto product = new()
            {
                Category = categoryId,
                Description = description,
                Image = image,
                Price = price,
                Title = title,
                Stock = stock

            };

            CreateProduct createProduct = new(dbContext);


            // ACT
            ProductEntity act = await createProduct.Run(product, UserRole.Admin);

            // Assert
            //await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{unexistingCategoryId}' not found");
            act.Status.Should().Be(ProductStatus.Created);
            act.Category.Id.Should().Be(categoryId);
            act.Description.Should().Be(description);
            act.Image.Should().Be(image);
            act.Title.Should().Be(title);
            act.Stock.Should().Be(stock);
        }
    }   
}
