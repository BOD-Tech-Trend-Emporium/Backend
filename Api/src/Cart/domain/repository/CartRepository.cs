using Api.src.Cart.domain.dto;

namespace Api.src.Cart.domain.repository
{
    public interface CartRepository
    {
        Task<CartResponse> CreateAsync(Guid idUser);
        Task<CartResponse> UpdateAsync(UpdateCartDto entity, Guid userId);
        Task<CartResponse> GetPendingCartAsync(Guid userId);
        Task<PurchaseResponse> CreatePurchaseAsync(Guid userId);
    }
}
