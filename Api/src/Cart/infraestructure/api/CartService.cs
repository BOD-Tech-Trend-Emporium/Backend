using Api.src.Cart.application.service;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.repository;
using Api.src.Cart.domain.entity;
using backend.Data;

namespace Api.src.Cart.infraestructure.api
{
    public class CartService : CartRepository
    {
        private CreateCart _createCart;
        private UpdateCartByUserId _updateCartByUserId;

        public CartService(ApplicationDBContext context)
        {
            _createCart = new CreateCart(context);
            _updateCartByUserId = new UpdateCartByUserId(context);

        }

        public async Task<CartEntity> CreateAsync(Guid idUser)
        {
            return await _createCart.Run(idUser);
        }

        public async Task<CartEntity> UpdateAsync(UpdateCartDto entity, Guid userId)
        {
            return await _updateCartByUserId.Run(entity, userId);
        }
    }
}
