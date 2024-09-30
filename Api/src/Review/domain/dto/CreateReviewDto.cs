namespace Api.src.Review.domain.dto
{
    public class CreateReviewDto
    {
        public required Guid productId { get; set; }
        public string comment { get; set; }
        public float rating { get; set; }
    }
}
