using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.enums;

namespace Api.src.Category.application.service
{
    public class CreateCategory
    {
        private readonly ApplicationDBContext _context;

        public CreateCategory(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CategoryEntity> Run(CategoryEntity category, UserRole role)
        {

            if (_context.Category.Any(c => c.Name == category.Name))
            {
                throw new ConflictException("Category already exists");
            }

            var categoryStatus = role == UserRole.Admin ? CategoryStatus.Created : CategoryStatus.ToCreate;

            category.Status = categoryStatus;
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
         
        }
    }
}
