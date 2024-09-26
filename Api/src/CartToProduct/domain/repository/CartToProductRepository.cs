using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;

namespace Api.src.CartToProduct.domain.repository
{
    public interface CartToProductRepository
    {
        Task<CartToProductEntity> CreateAsync(CreateCartToProductDto createCartToProductDto, Guid userId);
        Task<DeleteCartToProductByProductIdDtoResponse> DeleteByIdProductAsync(Guid productId, Guid userId);
    }
}
