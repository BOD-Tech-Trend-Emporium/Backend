using Api.src.Cart.application.mappers;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Coupon.application.service;
using backend.Data;
using backend.src.User.application.service;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.src.Cart.application.service
{
    public class GetPendingCartByUserId
    {
        private readonly ApplicationDBContext _context;
        private readonly GetUserById _getUserById;
        private readonly GetCouponByCode _getCouponByCode;
        public GetPendingCartByUserId(ApplicationDBContext context)
        {
            _context = context;
            _getUserById = new GetUserById(context);
            _getCouponByCode = new GetCouponByCode(context);
        }

        public async Task<CartResponse> Run(Guid idUser)
        {
            var user = await _getUserById.Run(idUser);
            var cartEntity = await _context.Cart.Include(c => c.Coupon).FirstOrDefaultAsync(c => c.User.Id == user.Id && c.State == CartState.Pending);
            /*await _context.Entry(cartEntity)
                .Reference(c => c.Coupon)
                .LoadAsync();*/

            if (cartEntity == null)
            {
                throw new NotFoundException("Cart does not have products");
            }
            var shoppingCart = await _context.CartToProduct.Where(cp => cp.CartId == cartEntity.Id).Select(cp => cp.Price.Product.Id).ToListAsync();
            var totalBeforeDiscount = await _context.CartToProduct.Where(cp => cp.CartId == cartEntity.Id).Select(cp => cp.Quantity * cp.Price.Price).SumAsync();

            if (cartEntity.Coupon== null) {
                CartResponse cartResponse = CartMapper.ToCartResponse(cartEntity);
                cartResponse.Shopping_cart = shoppingCart;
                cartResponse.Final_total = totalBeforeDiscount + cartResponse.Shipping_cost;

                return cartResponse;
            }

            var couponEntity = await _getCouponByCode.Run(cartEntity.Coupon.Code);

            CartWithCouponDtoResponse cartWithCouponDtoResponse = CartMapper.ToCartWithCouponDtoResponse(cartEntity);
            cartWithCouponDtoResponse.Shopping_cart = shoppingCart;
            cartWithCouponDtoResponse.Total_before_discount = totalBeforeDiscount;
            cartWithCouponDtoResponse.Total_after_discount = cartWithCouponDtoResponse.Total_before_discount - (cartWithCouponDtoResponse.Total_before_discount * cartWithCouponDtoResponse.Coupon_applied.Discount_percentage) / 100;
            cartWithCouponDtoResponse.Final_total = cartWithCouponDtoResponse.Total_after_discount + cartWithCouponDtoResponse.Shipping_cost;
            return cartWithCouponDtoResponse;
        }

    }
}
