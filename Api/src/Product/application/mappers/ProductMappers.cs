using Api.src.Category.domain.entity;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
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

    }
}
