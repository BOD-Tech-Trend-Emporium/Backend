using Api.src.Cart.domain.dto;

namespace Api.src.Cart.domain.repository
{
    public interface CartRepository
    {
        Task<CartResponseDto> CreateAsync(Guid idUser);
        Task<CartResponseDto> UpdateAsync(UpdateCartDto entity, Guid userId);
        Task<CartResponseDto> GetPendingCartAsync(Guid userId);
        Task<PurchaseResponseDto> CreatePurchaseAsync(Guid userId);
    }
}
