namespace Api.src.Cart.domain.dto
{
    public class CartWithCouponDtoResponse : CartResponse
    {
        public CouponDto CouponApplied { get; set; }
        public float TotalBeforeDiscount { get; set; }
        public float TotalAfterDiscount { get; set; }


    }
}
