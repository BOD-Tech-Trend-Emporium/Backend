using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.entity;
using Api.src.Coupon.domain.entity;
using backend.Data;
using backend.src.User.domain.dto;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
        public static CartWithCouponDtoResponse ToCartWithCouponDtoResponse( CartEntity cart) {
            return new CartWithCouponDtoResponse
            {
                UserId = cart.User.Id,
                Coupon_applied = ToCouponDto(cart.Coupon),
                Shipping_cost = cart.ShippingCost,

            };
        }

        public static CartResponse ToCartResponse(CartEntity cart)
        {
            return new CartResponse
            {
                UserId = cart.User.Id,
                Shipping_cost = cart.ShippingCost,

            };
        }

        public static CouponDto ToCouponDto(CouponEntity couponEntity) {
            return new CouponDto
            {
                Coupon_code = couponEntity.Code,
                Discount_percentage = couponEntity.Discount
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
