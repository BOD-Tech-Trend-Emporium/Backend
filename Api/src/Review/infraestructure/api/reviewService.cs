using Api.src.Review.application.service;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using Api.src.Review.domain.repository;
using backend.Data;
using Microsoft.Identity.Client;

namespace Api.src.Review.infraestructure.api
{
    public class reviewService : ReviewRepository
    {
        private CreateReview createReviewService;
        private SearchReviewByProduct searchReviewByProductService;
        private UpdateReview updateReviewService;

        public reviewService(ApplicationDBContext context) 
        {
            createReviewService = new CreateReview(context);
            searchReviewByProductService = new SearchReviewByProduct(context);
            updateReviewService = new UpdateReview(context);
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
            return await searchReviewByProductService.Run(id);
        }

        public async Task<List<ReviewEntity>> GetAllByUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ReviewEntity> UpdateAsync(UpdateReviewDto review, Guid userId, Guid reviewId)
        {
            return await updateReviewService.Run(review, userId, reviewId);
        }
    }
}
