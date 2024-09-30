using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;

namespace Api.src.CartToProduct.application.mappers
{
    public static class CartToProductMapper
    {
        public static CartToProductDto ToCartProductDto(this CartToProductEntity cartToProductEntity)
        {
            return new CartToProductDto
            {
                PriceId = cartToProductEntity.PriceId,
                CartId = cartToProductEntity.CartId,
            };
        }
    }
}
