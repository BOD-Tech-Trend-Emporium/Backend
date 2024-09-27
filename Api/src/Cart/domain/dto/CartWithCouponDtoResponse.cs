namespace Api.src.Cart.domain.dto
{
    public class CartWithCouponDtoResponse : CartResponse
    {
        public Guid UserId { get; set; }
        public List<Guid> Shopping_cart { get; set; }
        public CouponDto Coupon_applied { get; set; }
        public float Total_before_discount { get; set; }
        public float Total_after_discount { get; set; }
        public float Shipping_cost { get; set; }
        public float Final_total { get; set; }


    }
}
