using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Product.application.mappers;
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

namespace Test.Product.UnitTest.mappers.UnitTest
{
    public class toProductDtoTest
    {
        [Fact]
        public void Given_ProductEntity_When_IsCorrect__Then_ProductDto()
        {
            Guid categoryId = Guid.NewGuid();
            var categoryName = "Books";
            CategoryEntity category1 = new()
            {
                Id = categoryId,
                Name = categoryName,
                Status = CategoryStatus.Created
            };

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
            // ACT
            ProductDto act = product.ToProductDto(0f,30,4f,categoryName);

            // Assert
            act.Id.Should().Be(prId);
            act.Category.Should().Be(categoryName);
            act.Description.Should().Be(description);
            act.Image.Should().Be(image);
            act.Title.Should().Be(title);
            act.Price.Should().Be(0);
            act.Rating.Count.Should().Be(30);
            act.Rating.Rate.Should().Be(4f);
        }
    }
}
