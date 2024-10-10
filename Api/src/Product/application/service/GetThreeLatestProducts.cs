using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Product.application.service
{
    public class GetThreeLatestProducts
    {
        private readonly ApplicationDBContext _context;

        public GetThreeLatestProducts(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDto>> Run() {
            var products = await _context.Product
            .Where(pr => pr.Status.Equals(ProductStatus.Created))
            .OrderByDescending(p => p.CreatedAt)
            .Include(p => p.Category) // Cargar la categoría relacionada
            .Include(p => p.Prices) // Cargar los precios relacionados
            .Include(p => p.Reviews)// Cargar las reseñas relacionadas
            .Take(3)
            .ToListAsync();
            return products.Select(product => {
                var currentPrice = product.Prices?.Where(pr => pr.Current).Select(pr => pr.Price).FirstOrDefault() ?? 0f;
                var numberOfRatings = product.Reviews?.Count ?? 0;
                var avgRating = product.Reviews?.Count == 0 ? 0f : product.Reviews.Average(rev => rev.Rating) ?? 0f;
                var categoryName = product.Category?.Name ?? "Unknown";
                return product.ToProductDto(
                    // select the current price
                    currentPrice,
                    // select the number of rattings
                    numberOfRatings,
                    // Calculate the average (in case of no ratings, send 0)
                    avgRating,
                    // Select the name of the category
                    categoryName
                    );
            }).ToList();
        }
    }
}
