﻿using Api.src.Cart.domain.enums;
using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;
using Api.src.Common.exceptions;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.CartToProduct.application.service
{
    public class DeleteCartToProductByProductId
    {
        private readonly ApplicationDBContext _context;

        public DeleteCartToProductByProductId(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<DeleteCartToProductByProductIdResponseDto> Run(Guid productId, Guid userIde)
        {
            var cartEntity = await _context.Cart.FirstOrDefaultAsync(c => c.User.Id == userIde && c.State == CartState.Pending) ?? throw new NotFoundException("User does not have a cart");
            var productEntity = await _context.Product.FirstOrDefaultAsync(p => p.Id == productId && (p.Status == ProductStatus.Created || p.Status == ProductStatus.ToRemove)) ?? throw new NotFoundException("Product does not exist");
            var priceEntity = await _context.Price.FirstOrDefaultAsync(p => p.Product.Id == productEntity.Id && p.Current == true) ?? throw new NotFoundException("Price of product does not exist");

            var cartToProducEntity = await _context.CartToProduct.FirstOrDefaultAsync(cp => cp.CartId == cartEntity.Id && cp.PriceId==priceEntity.Id) ?? throw new NotFoundException("The cart does not have the product");
            
            _context.CartToProduct.Remove(cartToProducEntity);
            await _context.SaveChangesAsync();

            if (!await _context.CartToProduct.AnyAsync(c => c.CartId == cartEntity.Id)) { 
                cartEntity.Coupon = null;
                _context.Cart.Remove(cartEntity);
                await _context.SaveChangesAsync();

            }

            return new DeleteCartToProductByProductIdResponseDto() { Message = "The product was deleted successfully"};
        }
    }
}
