using Api.src.Category.domain.entity;
using Api.src.Common.exceptions;
using backend.Data;

namespace Api.src.Category.application.service
{
    public class CreateCategory
    {
        private readonly ApplicationDBContext _context;

        public CreateCategory(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CategoryEntity> Run(CategoryEntity category)
        {

            if (_context.Category.Any(c => c.Name == category.Name)) {
                throw new DuplicateException("User already exists");
            }
            category.Status = domain.enums.CategoryStatus.Created;
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
         
        }
    }
}
