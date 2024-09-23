using Api.src.Category.domain.entity;
using backend.src.User.domain.enums;
namespace Api.src.Category.domain.repository
{
    public interface CategoryRepository
    {
        Task<CategoryEntity> CreateAsync(CategoryEntity category, UserRole role);
        Task<List<CategoryEntity>> GetAllApprovedAsync();
    }
}
