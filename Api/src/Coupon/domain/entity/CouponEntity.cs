using Api.src.Coupon.domain.enums;
using Api.src.Favorite.domain.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.src.Coupon.domain.entity
{
    public class CouponEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Code { get; set; }

        [Required, Column(TypeName = "FLOAT")]
        public float Discount { get; set; }

        [Required]
        public CouponStatus Status { get; set; }

        public ICollection<CartEntity> Carts { get; set; }
    }
}
