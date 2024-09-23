
namespace Api.src.Product.domain.dto
{
    public class CreateProductDto
    {
        public required string Title { get; set; }
        public float Price { get; set; }
        public required string Description { get; set; }
        public required Guid Category { get; set; }
        public required string Image { get; set; }
        public int Stock { get; set; }
    }
}
