using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using backend.src.User.domain.enums;

namespace Api.src.Product.domain.repository
{
    public interface ProductRepository
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<List<ProductDto>> SearchAsync(SearchProductsDto query);
        Task<ProductByIdDto> GetByIdAsync(Guid id);
        Task<ProductEntity> CreateAsync(CreateProductDto entity, UserRole role);
        Task<ProductEntity> DeleteByIdAsync(Guid id);
        Task<ProductEntity> UpdateByIdAsync(CreateProductDto entity, Guid id);
        Task<List<ProductDto>> GetThreeLatestAsync();
    }
}
