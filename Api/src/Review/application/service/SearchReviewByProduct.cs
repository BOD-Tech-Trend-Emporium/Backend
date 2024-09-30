using Api.src.Common.exceptions;
using Api.src.Review.domain.entity;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Review.application.service
{
    public class SearchReviewByProduct
    {
        private readonly ApplicationDBContext _context;

        public SearchReviewByProduct(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewEntity>> Run(Guid productId)
        {
            var product = await _context.Product
                .FirstOrDefaultAsync(product => product.Id == productId);

            if (product == null)
            {
                throw new BadRequestException(
                    $"product with id {productId} doesn't exist"
                    );
            }
            var reviews = await _context.Review
                .Where(rev => rev.Product.Id.Equals(productId))
                .Include(rev => rev.Product )
                .Include(rev => rev.User)
                .ToListAsync();

            return reviews;
        }

    }
}
