using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Product.application.service
{
    public class GetBestSellingProducts
    {
        private readonly ApplicationDBContext _context;
        private readonly GetProductById _getProductById;
        private readonly GetAllProducts _getAllProducts;

        public GetBestSellingProducts(ApplicationDBContext context)
        {
            _context = context;
            _getProductById = new GetProductById(context);
            _getAllProducts = new GetAllProducts(context);
        }
        public async Task<List<ProductDto>> Run()
        {
            var products = await _getAllProducts.Run();
            List<ProductDto> response = new List<ProductDto>();
            List<ProductByIdDto> result = new List<ProductByIdDto>();
            foreach (var product in products)
            {
                result.Add(await _getProductById.Run(product.Id));
            }
            var orderResult = result.OrderByDescending(r => r.Inventory.Total - r.Inventory.Available).Take(3).ToList();

            foreach (var id in orderResult.Select(or => or.Id))
            {
                response.Add(products.FirstOrDefault(p => p.Id == id));
            }
            return response;
        }
    }
}
