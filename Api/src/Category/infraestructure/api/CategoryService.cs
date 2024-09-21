using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.repository;
using backend.Data;
using backend.src.User.domain.enums;

namespace Api.src.Category.infraestructure.api
{
    public class CategoryService : CategoryRepository
    {
        private GetAllApprovedCategories getAllApprovedCategories;
        private CreateCategory createCategory;
        public CategoryService(ApplicationDBContext context)
        {
            getAllApprovedCategories = new GetAllApprovedCategories(context);
            createCategory = new CreateCategory(context);
        }

        public async Task<CategoryEntity> CreateAsync(CategoryEntity category, UserRole role)
        {
            return await createCategory.Run(category, role);
        }

        public async Task<List<CategoryEntity>> GetAllApprovedAsync()
        {
            return await getAllApprovedCategories.Run();
        }

    }
}
