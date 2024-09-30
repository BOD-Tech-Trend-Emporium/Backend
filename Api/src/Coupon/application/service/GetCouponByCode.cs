using Api.src.Common.exceptions;
using Api.src.Coupon.domain.entity;
using Api.src.Coupon.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Coupon.application.service
{
    public class GetCouponByCode
    {
        private readonly ApplicationDBContext _context;

        public GetCouponByCode(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<CouponEntity> Run(string couponCode)
        {
            var couponEntity = await _context.Coupon.FirstOrDefaultAsync(c => c.Code.Equals(couponCode) && c.Status == CouponStatus.Active);
            if (couponCode == null || couponEntity == null)
            {
                throw new NotFoundException("Coupon does not exist");
            }

            return couponEntity;

        }

    }
}
