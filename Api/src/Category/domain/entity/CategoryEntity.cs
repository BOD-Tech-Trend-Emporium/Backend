using Api.src.Category.domain.enums;
using Api.src.Product.domain.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.src.Category.domain.entity
{
    public class CategoryEntity
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters"), Column(TypeName = "VARCHAR"), Required]
        public string Name { get; set; }

        [Required]
        public CategoryStatus Status { get; set; }

        public ICollection<ProductEntity> Products { get; set; }
    }
}
