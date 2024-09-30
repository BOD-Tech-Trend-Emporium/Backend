namespace Api.src.Review.domain.dto
{
    public class UpdateReviewDto
    {
        public required Guid userId { get; set; }
        public string comment { get; set; }
        public float rating { get; set; }
    }
}
