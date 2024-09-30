using Api.src.Category.domain.dto;
using Api.src.Common.exceptions;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Review.application.service
{
    public class CreateReview
    {
        private readonly ApplicationDBContext _context;

        public CreateReview(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ReviewEntity> Run(CreateReviewDto review, Guid userId) 
        {
            var product = await _context.Product
                .FirstOrDefaultAsync(product => product.Id == review.productId);

            var user = await _context.User
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (product == null || user == null)
            {
                throw new BadRequestException(
                    $"user with Id {userId} or product with id {review.productId} doesn't exist"
                    );
            }

            ReviewEntity entity = new ReviewEntity {
                Comment = review.comment,
                Product = product,
                Rating = review.rating,
                User = user,
            };
            _context.Review.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

    }
}
