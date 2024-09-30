using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;

namespace Api.src.Review.domain.repository
{
    public interface ReviewRepository
    {
        Task<List<ReviewEntity>> GetAllByUserAsync(Guid id);
        Task<List<ReviewEntity>> GetAllByProductAsync(Guid id);
        Task<ReviewEntity> CreateAsync(CreateReviewDto review, Guid userId);
        Task<ReviewEntity> UpdateAsync(ReviewEntity entity);
        Task<ReviewEntity> DeleteAsync(Guid id);
    }
}
