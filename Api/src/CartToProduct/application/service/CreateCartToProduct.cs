using Api.src.Cart.domain.enums;
using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.entity;
using Api.src.Common.exceptions;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.CartToProduct.application.service
{
    public class CreateCartToProduct
    {
        private readonly ApplicationDBContext _context;

        public CreateCartToProduct(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<CartToProductEntity> Run(CreateCartToProductDto createCartToProductDto, Guid userIde)
        {
            var cartEntity = await _context.Cart.FirstOrDefaultAsync(c => c.User.Id == userIde && c.State == CartState.Pending) ?? throw new NotFoundException("User does not have a cart");
            var productEntity = await _context.Product.FirstOrDefaultAsync(p => p.Id == createCartToProductDto.ProductId && (p.Status ==ProductStatus.Created || p.Status ==ProductStatus.ToRemove)) ?? throw new NotFoundException("Product does not exist");
            var priceEntity = await _context.Price.FirstOrDefaultAsync(p => p.Product.Id == productEntity.Id && p.Current == true) ?? throw new NotFoundException("Price of product does not exist");
            if (await _context.CartToProduct.AnyAsync(c => c.PriceId == priceEntity.Id && c.CartId == cartEntity.Id)) {
                throw new ConflictException("The product is already in the cart");
            }
            if (createCartToProductDto.Quantity < 1) {
                throw new ConflictException("Invalid quantity");
            }
            var cartToProduct = new CartToProductEntity() { Price = priceEntity, Cart = cartEntity, Quantity=createCartToProductDto.Quantity };

            await _context.CartToProduct.AddAsync(cartToProduct);
            await _context.SaveChangesAsync();
            return cartToProduct;
        }
    }
}
