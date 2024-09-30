using Api.src.Category.domain.dto;
using Api.src.Common.exceptions;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            // check review
            if (review.rating < 0 || review.rating>5)
            {
                throw new BadRequestException(
                    $"rating must be between 0 and 5, given: {review.rating}"
                    );
            }

            // Check if the product and the user exists
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

            // Check if the user has already make a review on this product

            var existingReview = await _context.Review
                .FirstOrDefaultAsync(r => r.User.Id == userId && r.Product.Id == review.productId);

            if (existingReview != null)
            {
                throw new BadRequestException(
                    $"User with Id {userId} has already reviewed the Product with Id {review.productId}. User should update instead of create review"
                );
            }

            // Create the review
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
