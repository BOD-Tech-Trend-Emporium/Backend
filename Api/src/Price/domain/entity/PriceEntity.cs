using Api.src.CartToProduct.domain.entity;
using Api.src.Product.domain.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.src.Price.domain.entity
{
    public class PriceEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public ProductEntity Product { get; set; }

        [Required, Column(TypeName = "FLOAT")]
        public float Price { get; set; }

        [Required]
        public bool Current { get; set; }

        [Required, Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; }

        public ICollection<CartToProductEntity> CartToProducts { get; set; }
    }
}
