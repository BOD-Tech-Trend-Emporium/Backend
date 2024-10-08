﻿using Api.src.Cart.domain.enums;
using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;
using Api.src.Common.exceptions;
using Api.src.Product.application.service;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.CartToProduct.application.service
{
    public class UpdateCartToProduct
    {
        private readonly ApplicationDBContext _context;
        private readonly GetProductById _getProductById;
        public UpdateCartToProduct(ApplicationDBContext context)
        {
            _context = context;
            _getProductById = new GetProductById(context);
        }
        public async Task<UpdateCartToProductResponseDto> Run(UpdateCartToProductDto updateCartToProductDto, Guid userIde)
        {
            var cartEntity = await _context.Cart.FirstOrDefaultAsync(c => c.User.Id == userIde && c.State == CartState.Pending) ?? throw new NotFoundException("User does not have a cart");
            var productEntity = await _context.Product.FirstOrDefaultAsync(p => p.Id == updateCartToProductDto.ProductId && (p.Status == ProductStatus.Created || p.Status == ProductStatus.ToRemove)) ?? throw new NotFoundException("Product does not exist");
            var priceEntity = await _context.Price.FirstOrDefaultAsync(p => p.Product.Id == productEntity.Id && p.Current == true) ?? throw new NotFoundException("Price of product does not exist");
            var cartToProducEntity = await _context.CartToProduct.FirstOrDefaultAsync(cp => cp.CartId == cartEntity.Id && cp.PriceId == priceEntity.Id) ?? throw new NotFoundException("The cart does not have the product");
            if (updateCartToProductDto.Quantity < 1)
            {
                throw new ConflictException("Invalid quantity");
            }
            var productResult = await _getProductById.Run(productEntity.Id);
            var available = productResult.Inventory.Available;
            if (updateCartToProductDto.Quantity > available)
            {
                throw new ConflictException("Insufficient product in inventory");
            }
            cartToProducEntity.Quantity = updateCartToProductDto.Quantity;

            await _context.SaveChangesAsync();
            return new UpdateCartToProductResponseDto() { Message = "Update successfully" };
        }
    }
}
