using Api.src.Review.domain.entity;
using Api.src.Review.domain.repository;
using backend.Data;

namespace Api.src.Review.infraestructure.api
{
    public class reviewService : ReviewRepository
    {
        public reviewService(ApplicationDBContext context) 
        {
        
        }
        public Task<ReviewEntity> CreateAsync(ReviewEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewEntity> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReviewEntity>> GetAllByProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReviewEntity>> GetAllByUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewEntity> UpdateAsync(ReviewEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
