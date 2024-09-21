using Api.src.Category.domain.entity;
using Api.src.Common.exceptions;
using Api.src.Price.domain.entity;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using backend.Data;
using backend.src.User.domain.enums;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Api.src.Product.application.mappers
{
    public static class ProductMappers
    {
        public static ProductDto ToProductDto( this ProductEntity product, float price, int count, float rate, string category )
        {
            return new ProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Category = category,
                Image = product.Image,
                Price = price,
                Rating = new ProductRatingDto
                {
                    Count = count,
                    Rate = rate
                }
            };
        }
        public static async Task<ProductEntity> ToProductModelForCreate(this CreateProductDto product, ApplicationDBContext _context, UserRole role)
        {
            // search category Id
            var category = await _context.Category
                .FirstOrDefaultAsync(cat => cat.Name == product.Category);

            if (category == null)
            {
                throw new NotFoundException($"Category '{product.Category}' not found");
            }
            // Create product
            var productEntity = new ProductEntity
            {
                Title = product.Title,
                Description = product.Description,
                Image = product.Image,
                Stock = product.Stock,
                Status = role == UserRole.Admin ? ProductStatus.Created : ProductStatus.ToCreate,
                Category = category,
                // TODO: DELETE THIS FIELD WHEN UPDATE DDBB
                Price= product.Price,
                Prices = new List<PriceEntity>()
            };
            var priceEntity = new PriceEntity
            {
                Product = productEntity,
                Price = product.Price,
                Current = true,
            };
            productEntity.Prices.Add(priceEntity);

            return productEntity;
        }

    }
}
