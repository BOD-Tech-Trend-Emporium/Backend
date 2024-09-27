namespace Api.src.Cart.domain.dto
{
    public class CartResponse
    {
        public Guid UserId { get; set; }
        public List<Guid> Shopping_cart { get; set; }
        public float Shipping_cost { get; set; }
        public float Final_total { get; set; }
    }
}
