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

        public CartService(ApplicationDBContext context)
        {
            _createCart = new CreateCart(context);

        }

        public async Task<CartEntity> CreateAsync(Guid idUser)
        {
            return await _createCart.Run(idUser);
        }
    }
}
