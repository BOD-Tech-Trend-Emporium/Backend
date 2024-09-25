using Api.src.Category.domain.dto;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.enums;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Category.application.service
{
    public class UpdateCategoryById
    {
        private readonly ApplicationDBContext _context;

        public UpdateCategoryById(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UpdateCategoryByIdResponse> Run(Guid id,CategoryEntity category, UserRole role)
        {
            var CategoryToUpdate = await _context.Category.FirstOrDefaultAsync(c => c.Id ==  id &&(c.Status ==CategoryStatus.Created || c.Status ==CategoryStatus.ToRemove));
            if (CategoryToUpdate == null)
            {
                throw new NotFoundException($"Category does not exist with id {id}");
            }
            if (await _context.Category.AnyAsync(c => c.Name ==category.Name))
            {
                throw new ConflictException("Category already exists");
            }

            CategoryToUpdate.Name = category.Name;
            await _context.SaveChangesAsync();

            return new UpdateCategoryByIdResponse() { Message = "Updated successfuly"};

        }
    }

}

