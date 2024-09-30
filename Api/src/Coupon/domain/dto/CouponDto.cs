namespace Api.src.Coupon.domain.dto
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public float Discount { get; set; }

    }
}
