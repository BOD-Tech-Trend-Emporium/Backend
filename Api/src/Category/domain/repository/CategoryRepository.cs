using Api.src.Category.domain.entity;
namespace Api.src.Category.domain.repository
{
    public interface CategoryRepository
    {
        Task<CategoryEntity> CreateAsync(CategoryEntity category);
        Task<List<CategoryEntity>> GetAllApprovedAsync();
    }
}
