using Api.src.Cart.application.mappers;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.enums;
using Api.src.Cart.domain.entity;
using backend.Data;
using backend.src.User.application.service;
using Microsoft.EntityFrameworkCore;
using Api.src.Common.exceptions;
using Api.src.Coupon.domain.enums;
using Api.src.Coupon.application.service;

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

        public async Task<CartEntity> Run(Guid idUser)
        {
            //var cartEntity = await CartMapper.ToCartModelForCreate(cart,_context);
            var cartEntity = new CartEntity();
            //var couponEntity = await _getCouponByCode.Run(cart.CouponCode);
            var user = await _getUserById.Run(idUser);
            if (await _context.Cart.AnyAsync(c => c.User.Id == idUser && c.State == CartState.Pending)) {
                throw new ConflictException("The cart already exists");
            }

            cartEntity.User = user;
            cartEntity.ShippingCost = new Random().Next(100, 1000);
            cartEntity.State = CartState.Pending;
            await _context.Cart.AddAsync(cartEntity);
            await _context.SaveChangesAsync();
            return cartEntity;
        }
    }
}
