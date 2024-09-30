using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Category.domain.entity;
using Api.src.Price.domain.entity;
using Api.src.Product.application.service;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using backend.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Test.src.Product.UnitTest.service.UnitTest
{
    public class SearchProductsTest
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
        public async void Given_CategoryName_When_UsingCategoryInQuery_Then_ReturnsOneCoincidence() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            CategoryEntity category1 = new(){
                Name = "Category1",
            };
            CategoryEntity category2 = new(){
                Name = "Category2",
            };
            ProductEntity product1 = new(){
                Title = "product1",
                Description = "product1",
                Image = "product1",
                Stock = 1,
                Status = ProductStatus.Created,
                Category = category1,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 1,
                Prices = []
            };
            ProductEntity product2 = new(){
                Title = "product2",
                Description = "product2",
                Image = "product2",
                Stock = 2,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 2,
                Prices = []
            };
            PriceEntity price1 = new()
            {
                Product = product1,
                Price = product1.Price,
                Current = true,
            };
            PriceEntity price2 = new()
            {
                Product = product2,
                Price = product2.Price,
                Current = true,
            };

            dbContext.Category.Add(category1);
            dbContext.Category.Add(category2);
            product1.Prices.Add(price1);
            product2.Prices.Add(price2);
            dbContext.Price.Add(price1);
            dbContext.Price.Add(price2);
            dbContext.Product.Add(product1);
            dbContext.Product.Add(product2);
            await dbContext.SaveChangesAsync();
            
            SearchProductsDto query = new (){
                Category = category1.Name
            };

            SearchProducts searchProducts = new (dbContext);

            //ACT
            List<ProductDto> result = await searchProducts.Run(query);

            //Assert
            result.Should().NotBeEmpty()
            .And.HaveCount(1)
            .And.Subject.First().Id.Should().Be(product1.Id);
        }
    
        [Fact]
        public async void Given_TitleName_When_UsingTitleInQuery_Then_ReturnsTwoCoincidences() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            CategoryEntity category1 = new(){
                Name = "Category1",
            };
            CategoryEntity category2 = new(){
                Name = "Category2",
            };
            ProductEntity product1 = new(){
                Title = "product1",
                Description = "product1",
                Image = "product1",
                Stock = 1,
                Status = ProductStatus.Created,
                Category = category1,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 1,
                Prices = []
            };
            ProductEntity product2 = new(){
                Title = "product2",
                Description = "product2",
                Image = "product2",
                Stock = 2,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 2,
                Prices = []
            };
            ProductEntity product3 = new(){
                Title = "SomethingElse",
                Description = "product3",
                Image = "product3",
                Stock = 3,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 3,
                Prices = []
            };
            PriceEntity price1 = new()
            {
                Product = product1,
                Price = product1.Price,
                Current = true,
            };
            PriceEntity price2 = new()
            {
                Product = product2,
                Price = product2.Price,
                Current = true,
            };
            PriceEntity price3 = new()
            {
                Product = product2,
                Price = product3.Price,
                Current = true,
            };

            dbContext.Category.Add(category1);
            dbContext.Category.Add(category2);
            product1.Prices.Add(price1);
            product2.Prices.Add(price2);
            product3.Prices.Add(price3);
            dbContext.Price.Add(price1);
            dbContext.Price.Add(price2);
            dbContext.Product.Add(product1);
            dbContext.Product.Add(product2);
            dbContext.Product.Add(product3);
            await dbContext.SaveChangesAsync();
            
            SearchProductsDto query = new (){
                Title = "product"
            };

            SearchProducts searchProducts = new (dbContext);

            //ACT
            List<ProductDto> result = await searchProducts.Run(query);

            //Assert
            result.Should().NotBeEmpty()
            .And.HaveCount(2);
            result.First().Id.Should().Be(product1.Id);
            result.ElementAt(1).Id.Should().Be(product2.Id);
        }
    
        [Fact]
        public async void Given_MinAndMaxPrice_When_UsingPriceInQuery_Then_ReturnsTwoCoincidences() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            CategoryEntity category1 = new(){
                Name = "Category1",
            };
            CategoryEntity category2 = new(){
                Name = "Category2",
            };
            ProductEntity product1 = new(){
                Title = "product1",
                Description = "product1",
                Image = "product1",
                Stock = 1,
                Status = ProductStatus.Created,
                Category = category1,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 5,
                Prices = []
            };
            ProductEntity product2 = new(){
                Title = "product2",
                Description = "product2",
                Image = "product2",
                Stock = 2,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 15,
                Prices = []
            };
            ProductEntity product3 = new(){
                Title = "SomethingElse",
                Description = "product3",
                Image = "product3",
                Stock = 3,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 10,
                Prices = []
            };
            PriceEntity price1 = new()
            {
                Product = product1,
                Price = product1.Price,
                Current = true,
            };
            PriceEntity price2 = new()
            {
                Product = product2,
                Price = product2.Price,
                Current = true,
            };
            PriceEntity price3 = new()
            {
                Product = product2,
                Price = product3.Price,
                Current = true,
            };

            dbContext.Category.Add(category1);
            dbContext.Category.Add(category2);
            product1.Prices.Add(price1);
            product2.Prices.Add(price2);
            product3.Prices.Add(price3);
            dbContext.Price.Add(price1);
            dbContext.Price.Add(price2);
            dbContext.Product.Add(product1);
            dbContext.Product.Add(product2);
            dbContext.Product.Add(product3);
            await dbContext.SaveChangesAsync();
            
            SearchProductsDto query = new (){
                MinPrice = 5,
                MaxPrice = 10,
            };

            SearchProducts searchProducts = new (dbContext);

            //ACT
            List<ProductDto> result = await searchProducts.Run(query);

            //Assert
            result.Should().NotBeEmpty()
            .And.HaveCount(2);
            result.First().Id.Should().Be(product1.Id);
            result.ElementAt(1).Id.Should().Be(product3.Id);
        }
    
        [Fact]
        public async void Given_PageSizeAndPage_When_UsingPriceInQuery_Then_ReturnsOneProduct() 
        {
            //Arrange
            var dbContext = await GetDataBaseContext();

            CategoryEntity category1 = new(){
                Name = "Category1",
            };
            CategoryEntity category2 = new(){
                Name = "Category2",
            };
            ProductEntity product1 = new(){
                Title = "product1",
                Description = "product1",
                Image = "product1",
                Stock = 1,
                Status = ProductStatus.Created,
                Category = category1,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 5,
                Prices = []
            };
            ProductEntity product2 = new(){
                Title = "product2",
                Description = "product2",
                Image = "product2",
                Stock = 2,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 15,
                Prices = []
            };
            ProductEntity product3 = new(){
                Title = "SomethingElse",
                Description = "product3",
                Image = "product3",
                Stock = 3,
                Status = ProductStatus.Created,
                Category = category2,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price = 10,
                Prices = []
            };
            PriceEntity price1 = new()
            {
                Product = product1,
                Price = product1.Price,
                Current = true,
            };
            PriceEntity price2 = new()
            {
                Product = product2,
                Price = product2.Price,
                Current = true,
            };
            PriceEntity price3 = new()
            {
                Product = product2,
                Price = product3.Price,
                Current = true,
            };

            dbContext.Category.Add(category1);
            dbContext.Category.Add(category2);
            product1.Prices.Add(price1);
            product2.Prices.Add(price2);
            product3.Prices.Add(price3);
            dbContext.Price.Add(price1);
            dbContext.Price.Add(price2);
            dbContext.Product.Add(product1);
            dbContext.Product.Add(product2);
            dbContext.Product.Add(product3);
            await dbContext.SaveChangesAsync();
            
            SearchProductsDto query = new (){
                Page = 2,
                PageSize = 2
            };

            SearchProducts searchProducts = new (dbContext);

            //ACT
            List<ProductDto> result = await searchProducts.Run(query);

            //Assert
            result.Should().NotBeEmpty()
            .And.HaveCount(1);
        }
    }
}