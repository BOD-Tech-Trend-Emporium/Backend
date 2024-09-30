using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;

namespace Api.src.Review.application.mappers
{
    public static class reviewMappers
    {
        public static ReviewDto ToReviewDto(this ReviewEntity review) 
        {
            return new ReviewDto { 
                comment = review.Comment,
                rating = review.Rating,
                id = review.Id,
                productId = review.Product.Id,
                userName = review.User.UserName
            }; 
        }
    }
}
