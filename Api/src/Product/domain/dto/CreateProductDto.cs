namespace Api.src.Product.domain.dto
{
    public class CreateProductDto
    {
        public string Title { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
    }
}
