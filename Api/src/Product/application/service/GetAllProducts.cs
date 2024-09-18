using Api.src.Product.domain.entity;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Product.application.service
{
    public class GetAllProducts
    {
        private readonly ApplicationDBContext _context;

        public GetAllProducts(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<ProductEntity>> Run()
        {
            return await _context.Product.ToListAsync();
        }
    }
}
