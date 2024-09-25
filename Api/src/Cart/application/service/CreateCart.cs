using Api.src.Cart.application.mappers;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.enums;
using Api.src.Cart.domain.entity;
using backend.Data;
using backend.src.User.application.service;
using Microsoft.EntityFrameworkCore;
using Api.src.Common.exceptions;
using Api.src.Coupon.domain.enums;

namespace Api.src.Cart.application.service
{
    public class CreateCart
    {
        private readonly ApplicationDBContext _context;
        private readonly GetUserById _getUserById;
        public CreateCart(ApplicationDBContext context)
        {
            _context = context;
            _getUserById = new GetUserById(context);
        }

        public async Task<CartEntity> Run(CreateCartDto cart, Guid idUser)
        {
            var cartEntity = await CartMapper.ToCartModelForCreate(cart,_context);
            var couponEntity = await _context.Coupon.FirstOrDefaultAsync(c => c.Code.Equals(cart.CouponCode) && c.Status == CouponStatus.Active);
            var user = await _getUserById.Run(idUser);
            cartEntity.User = user;
            if (cart.CouponCode != null && couponEntity == null) {
                throw new NotFoundException("Coupon does not exist");
            }
            cartEntity.Coupon = couponEntity;
            cartEntity.ShippingCost = new Random().Next(100, 1000);
            cartEntity.State = CartState.Pending;
            await _context.Cart.AddAsync(cartEntity);
            await _context.SaveChangesAsync();
            return cartEntity;
        }
    }
}
