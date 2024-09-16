using Api.src.Product.domain.entity;
using backend.src.User.domain.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.src.Favorite.domain.entity
{
    public class FavoriteEntity
    {
        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public ProductEntity Product { get; set; }

        [Required]
        public UserEntity User { get; set; }


        [Required, Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; }


    }
}
