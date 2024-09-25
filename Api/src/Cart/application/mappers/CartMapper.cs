using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.entity;
using backend.Data;
using backend.src.User.domain.dto;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Cart.application.mappers
{
    public static class CartMapper
    {
        public static CreateCartResponseDto ToCreateCartResponseDto(this CartEntity cart)
        {
            return new CreateCartResponseDto
            {
                Id = cart.Id,
                ShippingCost = cart.ShippingCost,
            };
        }
        public static UpdateCartResponseDto ToUpdateCartResponseDto(this CartEntity cart)
        {
            return new UpdateCartResponseDto
            {
                UserId = cart.User.Id,
                CouponId = cart.Coupon.Id,
            };
        }
        public static async Task<CartEntity> ToCartModelForUpdate(this UpdateCartDto cart, ApplicationDBContext context)
        {
            return new CartEntity
            {
                Coupon = await context.Coupon.FirstOrDefaultAsync(c => c.Code.Equals(cart.CouponCode)),
            };
        }
    }
}
