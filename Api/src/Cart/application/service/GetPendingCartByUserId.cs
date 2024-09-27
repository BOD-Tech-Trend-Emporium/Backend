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

            if (cartEntity == null)
            {
                throw new NotFoundException("Cart does not have products");
            }
            var shoppingCart = await _context.CartToProduct.Where(cp => cp.CartId == cartEntity.Id).Select(cp => cp.Price.Product.Id).ToListAsync();
            var totalBeforeDiscount = await _context.CartToProduct.Where(cp => cp.CartId == cartEntity.Id).Select(cp => cp.Quantity * cp.Price.Price).SumAsync();

            if (cartEntity.Coupon== null) {
                CartResponse cartResponse = CartMapper.ToCartResponse(cartEntity);
                cartResponse.ShoppingCart = shoppingCart;
                cartResponse.FinalTotal = totalBeforeDiscount + cartResponse.ShippingCost;

                return cartResponse;
            }

            var couponEntity = await _getCouponByCode.Run(cartEntity.Coupon.Code);

            CartWithCouponDtoResponse cartWithCouponDtoResponse = CartMapper.ToCartWithCouponDtoResponse(cartEntity);
            cartWithCouponDtoResponse.ShoppingCart = shoppingCart;
            cartWithCouponDtoResponse.TotalBeforeDiscount = totalBeforeDiscount;
            cartWithCouponDtoResponse.TotalAfterDiscount = cartWithCouponDtoResponse.TotalBeforeDiscount - (cartWithCouponDtoResponse.TotalBeforeDiscount * cartWithCouponDtoResponse.CouponApplied.Discount_percentage) / 100;
            cartWithCouponDtoResponse.FinalTotal = cartWithCouponDtoResponse.TotalAfterDiscount + cartWithCouponDtoResponse.ShippingCost;
            return cartWithCouponDtoResponse;
        }

    }
}
