using Api.src.Cart.domain.entity;
using Api.src.Price.domain.entity;
using System.ComponentModel.DataAnnotations;

namespace Api.src.CartToProduct.domain.entity
{
    public class CartToProductEntity
    {
        public Guid PriceId { get; set; }
        public Guid CartId { get; set; }

        [Required]
        public PriceEntity Price { get; set; }

        [Required]
        public CartEntity Cart { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
