using Api.src.Category.domain.dto;
using Api.src.Category.domain.enums;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.enums;
using System.Data;

namespace Api.src.Category.application.service
{
    public class DeleteCategoryById
    {
        private readonly ApplicationDBContext _context;

        public DeleteCategoryById(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<DeleteCategoryByIdResponseDto> Run(Guid id, UserRole role)
        {
            var category = _context.Category.Find(id);
            if (category == null || category.Status == CategoryStatus.Removed) {
                throw new NotFoundException($"Category with id {id} not found");
            }
            var categoryStatus = role == UserRole.Admin ? CategoryStatus.Removed : CategoryStatus.ToRemove;
            category.Status = categoryStatus;
            await _context.SaveChangesAsync();

            var message = role == UserRole.Admin ? "Deleted successfuly" : "Pending to delete";

            return new DeleteCategoryByIdResponseDto() { Message = message};
        }
    }
}
