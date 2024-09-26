using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;

namespace Api.src.CartToProduct.domain.repository
{
    public interface CartToProductRepository
    {
        Task<CartToProductEntity> CreateAsync(CreateCartToProductDto createCartToProductDto, Guid userId);
        Task<DeleteCartToProductByProductIdResponseDto> DeleteByIdProductAsync(Guid productId, Guid userId);
        Task<UpdateCartToProductResponseDto> UpdateAsync(UpdateCartToProductDto updateCartToProduct, Guid userId);
    }
}
