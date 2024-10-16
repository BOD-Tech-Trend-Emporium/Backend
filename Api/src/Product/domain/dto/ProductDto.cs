namespace Api.src.Product.domain.dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public float Stock { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public ProductRatingDto Rating { get; set; }

    }

    public class ProductRatingDto
    {
        public float Rate { get; set; }
        public int Count { get; set; }
    }
}

