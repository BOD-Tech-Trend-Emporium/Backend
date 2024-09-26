using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.enums;
using Microsoft.EntityFrameworkCore;

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
            var bdCategory = await _context.Category.FirstOrDefaultAsync(c => c.Name == category.Name);
            var categoryStatus = role == UserRole.Admin ? CategoryStatus.Created : CategoryStatus.ToCreate;
            if (bdCategory != null&&(bdCategory.Status == CategoryStatus.Created || bdCategory.Status == CategoryStatus.ToCreate))
            {
                throw new ConflictException("Category already exists or is pending to be created");
            }
            if (bdCategory != null && (bdCategory.Status == CategoryStatus.Removed || bdCategory.Status == CategoryStatus.ToRemove))
            {
                bdCategory.Status = categoryStatus;
                await _context.SaveChangesAsync();
                return bdCategory;
            }
            
            category.Status = categoryStatus;
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            
            return category;
         
        }
    }
}
