using Api.src.Product.domain.entity;
using backend.src.User.domain.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.src.Review.domain.entity
{
    public class ReviewEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public ProductEntity Product { get; set; }

        [Required]
        public UserEntity User { get; set; }

        public string? Comment { get; set; }

        [Column(TypeName = "FLOAT")]
        public float? Rating { get; set; }

        [Required, Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; }

    }
}
