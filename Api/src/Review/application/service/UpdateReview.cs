using Api.src.Common.exceptions;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Review.application.service
{
    public class UpdateReview
    {
        private readonly ApplicationDBContext _context;

        public UpdateReview(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ReviewEntity> Run(UpdateReviewDto review, Guid userId, Guid reviewId)
        {
            // check review
            if (review.rating < 0 || review.rating > 5)
            {
                throw new BadRequestException(
                    $"rating must be between 0 and 5, given: {review.rating}"
                    );
            }

            // Check if the review and the user exists
            var revToUpdate = await _context.Review
                // Make sure to load related information
                .Include(rev => rev.Product)
                .Include(rev => rev.User)
                .FirstOrDefaultAsync(rev => rev.Id == reviewId);

            var user = await _context.User
                .FirstOrDefaultAsync(user => user.Id == userId);

            if (revToUpdate == null || user == null)
            {
                throw new BadRequestException(
                    $"user with Id {userId} or review with id {reviewId} doesn't exist"
                    );
            }

            // Check if the user is the same creator

            if (!revToUpdate.User.Id.Equals(userId))
            {
                throw new BadRequestException(
                    $"User with Id {userId} doesnt have a request with ID {reviewId}"
                );
            }

            // Update the review
            revToUpdate.Comment = review.comment;
            revToUpdate.Rating = review.rating;
            await _context.SaveChangesAsync();
            return revToUpdate;
        }
    }
}
