using Api.src.Cart.domain.dto;
using Api.src.Category.domain.entity;
using Api.src.Cart.domain.entity;
using backend.src.User.domain.enums;

namespace Api.src.Cart.domain.repository
{
    public interface CartRepository
    {
        Task<CartEntity> CreateAsync(CreateCartDto cart, Guid idUser);
    }
}
