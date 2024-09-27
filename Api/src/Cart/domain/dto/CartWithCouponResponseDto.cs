namespace Api.src.Cart.domain.dto
{
    public class CartWithCouponResponseDto : CartResponse
    {
        public CouponDto CouponApplied { get; set; }
        public float TotalBeforeDiscount { get; set; }
        public float TotalAfterDiscount { get; set; }


    }
}
