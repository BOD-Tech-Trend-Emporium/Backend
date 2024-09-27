using Api.src.Coupon.domain.entity;

namespace Api.src.Coupon.domain.repository
{
    public interface CouponRepository
    {
        Task<CouponEntity> GetByCodeAsync(Guid id);
    }
}
