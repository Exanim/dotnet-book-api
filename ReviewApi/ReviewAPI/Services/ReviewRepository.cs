using Microsoft.EntityFrameworkCore;
using ReviewAPI.DbContexts;
using ReviewAPI.Entities;
using ReviewAPI.CustomErrors;

namespace ReviewAPI.Services
{
    public class ReviewRepository : IReviewRepository
    {

        private readonly ReviewContext _context;
        public ReviewRepository(ReviewContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync(
            int? userId, int? productId)
        {
            var collection = _context.Reviews as IQueryable<Review>;

            if (userId is not null)
            {
                collection = collection.Where(x => x.UserId == userId);
            }
            if (productId is not null)
            {
                collection = collection.Where(x => x.ProductId == productId);
            }

            return await collection.ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int ReviewId)
        {
            if (ReviewId < 1)
                throw new InvalidIdException("Invalid review ID! An ID has to be a positive integer", null);

            var result = await _context.Reviews
                .Where(review => review.ReviewId == ReviewId)
                .FirstOrDefaultAsync();

            if (result == null)
                throw new ReviewNotFoundException("Entry not found!", null);

            return result;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

    }
}
