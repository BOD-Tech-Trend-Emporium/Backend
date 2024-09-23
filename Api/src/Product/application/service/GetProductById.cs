using Api.src.Cart.domain.enums;
using Api.src.CartToProduct.domain.entity;
using Api.src.Common.exceptions;
using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Api.src.Product.application.service
{
    public class GetProductById
    {

        private readonly ApplicationDBContext _context;

        public GetProductById(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductByIdDto> Run(Guid id)
        {
            var product = await _context.Product
                .Where(pr => pr.Status.Equals(ProductStatus.Created) && pr.Id == id)
                .Include(p => p.Category) // Load related category
                .Include(p => p.Prices) // Load related prices
                    .ThenInclude(price => price.CartToProducts) // Load carts (helper table) related
                        .ThenInclude(priceToProducts => priceToProducts.Cart) // Load cart information related
                .Include(p => p.Reviews) // Load reviews related 
                .FirstOrDefaultAsync();
            
            // Send diferent state if the product is not available or wasnt found
            if (product == null)
            {
                throw new NotFoundException($"product with id {id.ToString()} not found");
            }
            if (product.Status == ProductStatus.Removed || product.Status == ProductStatus.ToCreate)
            {
                throw new NotFoundException($"product with id {id.ToString()} not available");
            }
            // Obtain metrics of the product
            // TODO: obtain availability
            var currentPrice = product.Prices?.Where(pr => pr.Current).Select(pr => pr.Price).FirstOrDefault() ?? 0f;
            var numberOfRatings = product.Reviews?.Count ?? 0;
            var avgRating = product.Reviews?.Count == 0 ? 0f : product.Reviews.Average(rev => rev.Rating) ?? 0f;
            var categoryName = product.Category?.Name ?? "Unknown";
            // Calculating availability
            /*
            var pendingProducts = product.Prices
                .Where(price => price.CartToProducts != null)
                .SelectMany(price => price.CartToProducts)  // Obtain all conections with CartToProduct
                .Where(ctp => ctp.Cart != null && ctp.Cart.State == CartState.Pending)  // Filter carts on pending state
                .Sum(ctp => (int?) ctp.Quantity ?? 7);  // Sum quantity of products
            */
            var count = 0;
            var pendingProducts = product.Prices
                .SelectMany(price => price.CartToProducts ?? new List<CartToProductEntity>())
                .Where(ctp => ctp.Cart != null && ctp.Cart.State == CartState.Approved)
                .Sum(ctp => (int?)ctp.Quantity ?? 0);
            Debug.WriteLine(count);
            // Calculate available products
            var stock = product.Stock;
            var availableProducts = stock - pendingProducts;
            Debug.WriteLine($"Stock: {product.Stock}");
            Debug.WriteLine($"Pending Products: {pendingProducts}");
            Debug.WriteLine($"Available Products: {availableProducts}");

            return product.toProductByIdDto(currentPrice, numberOfRatings, avgRating, categoryName, stock, availableProducts);
        }
    }
}
