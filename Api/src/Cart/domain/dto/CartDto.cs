using backend.src.User.domain.entity;

namespace Api.src.Cart.domain.dto
{
    public class CartDto
    {
        public UserEntity User { get; set; }
        public float ShippingCost { get; set; }

    }
}
