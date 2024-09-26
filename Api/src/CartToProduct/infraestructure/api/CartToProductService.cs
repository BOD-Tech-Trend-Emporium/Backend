using Api.src.CartToProduct.application.service;
using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;
using Api.src.CartToProduct.domain.repository;
using backend.Data;

namespace Api.src.CartToProduct.infraestructure.api
{
    public class CartToProductService : CartToProductRepository
    {
        private CreateCartToProduct _createCartToProduct;
        private DeleteCartToProductByProductId _deleteCartToProductByProductId;
        private UpdateCartToProduct _updateCartToProduct;

        public CartToProductService(ApplicationDBContext context)
        {
            _createCartToProduct = new CreateCartToProduct(context);
            _deleteCartToProductByProductId = new DeleteCartToProductByProductId(context);
            _updateCartToProduct = new UpdateCartToProduct(context);

        }
        public Task<CartToProductEntity> CreateAsync(CreateCartToProductDto createCartToProductDto, Guid userId)
        {
            return _createCartToProduct.Run(createCartToProductDto, userId);
        }

        public Task<DeleteCartToProductByProductIdResponseDto> DeleteByIdProductAsync(Guid productId, Guid userId)
        {
            return _deleteCartToProductByProductId.Run(productId, userId);
        }

        public Task<UpdateCartToProductResponseDto> UpdateAsync(UpdateCartToProductDto updateCartToProduct, Guid userId)
        {
            return _updateCartToProduct.Run(updateCartToProduct, userId);
        }
    }
}
