namespace Api.src.Review.domain.dto
{
    public class ReviewDto
    {
        public required Guid id { get; set; }
        public string? comment { get; set; }
        public float? rating { get; set; }
        public required Guid productId { get; set; }
        public required string userName { get; set; }
    }
}
