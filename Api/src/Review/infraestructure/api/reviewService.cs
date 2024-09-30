using Api.src.Review.application.service;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using Api.src.Review.domain.repository;
using backend.Data;

namespace Api.src.Review.infraestructure.api
{
    public class reviewService : ReviewRepository
    {
        private CreateReview createReviewService;
        public reviewService(ApplicationDBContext context) 
        {
            createReviewService = new CreateReview(context);
        }
        public async Task<ReviewEntity> CreateAsync(CreateReviewDto review, Guid userId)
        {
            return await createReviewService.Run(review, userId);
        }

        public async Task<ReviewEntity> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReviewEntity>> GetAllByProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReviewEntity>> GetAllByUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReviewEntity> UpdateAsync(ReviewEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
