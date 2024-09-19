using Api.src.Category.application.service;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.repository;
using backend.Data;

namespace Api.src.Category.infraestructure.api
{
    public class CategoryService : CategoryRepository
    {
        private GetAllApprovedCategories getAllApprovedCategories;

        public CategoryService(ApplicationDBContext context)
        {
            getAllApprovedCategories = new GetAllApprovedCategories(context);
        }

        public Task<CategoryEntity> CreateAsync(CategoryEntity category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryEntity>> GetAllApprovedAsync()
        {
            return await getAllApprovedCategories.Run();
        }
    }
}
