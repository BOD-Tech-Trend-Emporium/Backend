using Api.src.Category.application.service;
using Api.src.Category.domain.dto;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.repository;
using backend.Data;
using backend.src.User.domain.enums;

namespace Api.src.Category.infraestructure.api
{
    public class CategoryService : CategoryRepository
    {
        private GetAllApprovedCategories _getAllApprovedCategories;
        private CreateCategory _createCategory;
        private DeleteCategoryById _deleteCategory;
        public CategoryService(ApplicationDBContext context)
        {
            _getAllApprovedCategories = new GetAllApprovedCategories(context);
            _createCategory = new CreateCategory(context);
            _deleteCategory = new DeleteCategoryById(context);
        }

        public async Task<CategoryEntity> CreateAsync(CategoryEntity category, UserRole role)
        {
            return await _createCategory.Run(category, role);
        }

        public async Task<DeleteCategoryByIdResponseDto> DeleteCategoryByIdAsync(Guid id, UserRole role)
        {
            return await _deleteCategory.Run(id, role);
        }

        public async Task<List<CategoryEntity>> GetAllApprovedAsync()
        {
            return await _getAllApprovedCategories.Run();
        }

    }
}
