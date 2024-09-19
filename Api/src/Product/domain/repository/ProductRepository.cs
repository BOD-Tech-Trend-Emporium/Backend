using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;

namespace Api.src.Product.domain.repository
{
    public interface ProductRepository
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductEntity> GetByIdAsync(Guid id);
        Task<ProductEntity> CreateAsync(ProductEntity entity);
        Task<ProductEntity> DeleteByIdAsync(Guid id);
        // TODO Update to get with product DTO
        Task<ProductEntity> UpdateByIdAsync(Guid id);
    }
}
