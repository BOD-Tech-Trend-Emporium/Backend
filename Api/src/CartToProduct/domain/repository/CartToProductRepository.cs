using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;
using Api.src.Category.domain.entity;
using backend.src.User.domain.enums;

namespace Api.src.CartToProduct.domain.repository
{
    public interface CartToProductRepository
    {
        Task<CartToProductEntity> CreateAsync(CreateCartToProductDto createCartToProductDto, Guid userId);
    }
}
