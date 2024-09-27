namespace Api.src.Cart.domain.dto
{
    public class CartWithCouponDtoResponse : CartResponse
    {
        public CouponDto Coupon_applied { get; set; }
        public float Total_before_discount { get; set; }
        public float Total_after_discount { get; set; }


    }
}
