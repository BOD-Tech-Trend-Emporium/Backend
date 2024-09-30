namespace Api.src.Cart.domain.dto
{
    public class CartResponseDto
    {
        public Guid UserId { get; set; }
        public List<Guid> ShoppingCart { get; set; }
        public float ShippingCost { get; set; }
        public float FinalTotal { get; set; }
    }
}
