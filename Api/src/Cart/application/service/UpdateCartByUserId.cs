using Api.src.Cart.application.mappers;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.entity;
using Api.src.Cart.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Coupon.application.service;
using backend.Data;
using backend.src.User.application.service;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Cart.application.service
{
    public class UpdateCartByUserId
    {
        private readonly ApplicationDBContext _context;
        private readonly GetUserById _getUserById;
        private readonly GetCouponByCode _getCouponByCode;
        private readonly GetPendingCartByUserId _getPendingCartByUserId;
        public UpdateCartByUserId(ApplicationDBContext context)
        {
            _context = context;
            _getUserById = new GetUserById(context);
            _getCouponByCode = new GetCouponByCode(context);
            _getPendingCartByUserId = new GetPendingCartByUserId(context);
        }
        public async Task<CartResponse> Run(UpdateCartDto cart, Guid idUser)
        {
            var user = await _getUserById.Run(idUser);

            var cartEntity = await _context.Cart.FirstOrDefaultAsync(c => c.User.Id == user.Id && c.State ==CartState.Pending);
            if (cartEntity == null) {
                throw new NotFoundException("Cart does not exist");
            }
            var couponEntity = await _getCouponByCode.Run(cart.CouponCode);

            cartEntity.Coupon = couponEntity;
            await _context.SaveChangesAsync();

            return await _getPendingCartByUserId.Run(idUser);
            /*CartWithCouponDtoResponse cartWithCouponDtoResponse = CartMapper.ToCartWithCouponDtoResponse(cartEntity);
            cartWithCouponDtoResponse.Shopping_cart = await _context.CartToProduct.Where(cp => cp.CartId == cartEntity.Id).Select(cp => cp.Price.Product.Id).ToListAsync();
            cartWithCouponDtoResponse.Total_before_discount = await _context.CartToProduct.Where(cp => cp.CartId == cartEntity.Id).Select(cp => cp.Quantity * cp.Price.Price).SumAsync();
            cartWithCouponDtoResponse.Total_after_discount = cartWithCouponDtoResponse.Total_before_discount - (cartWithCouponDtoResponse.Total_before_discount * cartWithCouponDtoResponse.Coupon_applied.Discount_percentage)/100;
            cartWithCouponDtoResponse.Final_total = cartWithCouponDtoResponse.Total_after_discount + cartWithCouponDtoResponse.Shipping_cost;
            return cartWithCouponDtoResponse;*/
        }
    }
}
