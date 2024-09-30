using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.enums;
using Api.src.Common.exceptions;
using Api.src.Product.application.service;
using backend.Data;
using backend.src.User.application.service;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Cart.application.service
{
    public class CreatePurchase
    {
        private readonly ApplicationDBContext _context;
        private readonly GetUserById _getUserById;
        private readonly GetPendingCartByUserId _getPendingCartByUserId;
        private readonly GetProductById _getProductById;

        public CreatePurchase(ApplicationDBContext context)
        {
            _context = context;
            _getUserById = new GetUserById(context);
            _getPendingCartByUserId = new GetPendingCartByUserId(context);
            _getProductById = new GetProductById(context);
        }

        public async Task<PurchaseResponseDto> Run(Guid idUser)
        {
            var user = await _getUserById.Run(idUser);

            var cartEntity = await _context.Cart.FirstOrDefaultAsync(c => c.User.Id == user.Id && c.State == CartState.Pending);
            if (cartEntity == null)
            {
                throw new NotFoundException("Cart does not exist");
            }


            var cartToProduct = await _context.CartToProduct.Include(cp => cp.Price).ThenInclude(p => p.Product).Where(cp => cp.CartId == cartEntity.Id).ToListAsync();
            cartToProduct.ForEach(cp => {
                var productResult = _getProductById.Run(cp.Price.Product.Id).Result;
                var available = productResult.Inventory.Available;
                if (cp.Quantity > available)
                {
                    throw new ConflictException("Insufficient product in inventory");
                }

            });

            cartEntity.State = CartState.Approved;
            await _context.SaveChangesAsync();

            return new PurchaseResponseDto() { Message = "Successful purchase" };

        }

    }

}
