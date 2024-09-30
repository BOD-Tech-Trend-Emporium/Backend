using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Product.application.service
{
    public class SearchProducts
    {
        private readonly ApplicationDBContext _context;

        public SearchProducts(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Run(SearchProductsDto query)
        {
            var skipValue = (query.Page - 1) * query.PageSize;

            var products = await _context.Product
                .Where(pr => pr.Status.Equals(ProductStatus.Created))
                .Where(pr => pr.Title.Contains(query.Title))
                .Where(pr => pr.Prices.Any(price => price.Current && price.Price >= query.MinPrice && price.Price <= query.MaxPrice))
                .Where(pr => pr.Category.Name.Contains(query.Category))
                .Include(p => p.Category)
                .Include(p => p.Prices)
                .Include(p => p.Reviews)
                .Skip(skipValue).Take(query.PageSize).ToListAsync();
            
            var productsView = products.Select(product =>
            {
                var currentPrice = product.Prices?.Where(pr => pr.Current).Select(pr => pr.Price).FirstOrDefault() ?? 0f;
                var numberOfRatings = product.Reviews?.Count ?? 0;
                var avgRating = product.Reviews?.Count == 0 ? 0f : product.Reviews.Average(rev => rev.Rating) ?? 0f;
                var categoryName = product.Category?.Name ?? "Unknown";

                return product.ToProductDto(
                    currentPrice,
                    numberOfRatings,
                    avgRating,
                    categoryName
                    );
            }).ToList();


            return productsView;
        }
    }
}