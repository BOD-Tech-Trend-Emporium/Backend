using backend.src.User.domain.enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api.src.Cart.domain.enums;
using backend.src.User.domain.entity;
using Api.src.Coupon.domain.entity;
using Api.src.CartToProduct.domain.entity;

namespace Api.src.Favorite.domain.entity
{
    public class CartEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public UserEntity User { get; set; }

        [Required]
        public CouponEntity Coupon { get; set; }

        [Required, Column(TypeName = "FLOAT")]
        public float ShippingCost { get; set; }

        [Required]
        public CartState State { get; set; }

        [Required, Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; }

        public ICollection<CartToProductEntity> CartToProducts { get; set; }


    }
}
