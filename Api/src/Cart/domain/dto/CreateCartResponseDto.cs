using backend.src.User.domain.entity;

namespace Api.src.Cart.domain.dto
{
    public class CreateCartResponseDto
    {
        public Guid UserId { get; set; }
        public float ShippingCost { get; set; }
    }
}
