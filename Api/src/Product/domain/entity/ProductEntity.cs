using Api.src.Category.domain.entity;
using Api.src.Favorite.domain.entity;
using Api.src.Price.domain.entity;
using Api.src.Product.domain.enums;
using Api.src.Review.domain.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.src.Product.domain.entity
{
    public class ProductEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public CategoryEntity Category { get; set; }

        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters"), Column(TypeName = "VARCHAR"), Required]
        public string Title { get; set; }

        [Required, Column(TypeName = "FLOAT")]
        public float Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public ProductStatus Status { get; set; }

        [Required, Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; }

        public ICollection<ReviewEntity> Reviews { get; set; }

        public ICollection<FavoriteEntity> Favorites { get; set; }

        public ICollection<PriceEntity> Prices { get; set; }

    }
}
