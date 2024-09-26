using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Category.application.service
{
    public class GetAllApprovedCategories
    {
        private readonly ApplicationDBContext _context;

        public GetAllApprovedCategories(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryEntity>> Run()
        {
            var approvedCategories = await _context.Category.Where(category => category.Status == CategoryStatus.Created || category.Status == CategoryStatus.ToRemove).ToListAsync();
            return approvedCategories;
        }

    }
}
