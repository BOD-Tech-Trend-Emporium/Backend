using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Category.application.service
{
    public class GetCategoriesWithMostProducts
    {
        private readonly ApplicationDBContext _context;

        public GetCategoriesWithMostProducts(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryEntity>> Run()
        {
            return await _context.Category.Where(c => c.Status == CategoryStatus.Created || c.Status == CategoryStatus.ToRemove).OrderByDescending(p => p.Products.Count()).Take(3).ToListAsync();

        }
    }
}
