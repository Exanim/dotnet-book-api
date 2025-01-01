using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using RatingApi.DbContexts;
using RatingApi.Entities;

namespace RatingApi.Services
{
    public class RatingRepository : IRatingRepository
    {

        private readonly RatingContext _context;
        public RatingRepository(RatingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Rating>> GetRatings(
            int? userId, int? productId)
        {
            var collection = _context.Ratings as IQueryable<Rating>;

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

        public async Task<Rating> GetRatingById(int id)
        {
            return await _context.Ratings
                .Where(Rating => Rating.Id == id)
                .FirstOrDefaultAsync();
        }

        public void AddRating(Rating rating)
        {
            _context.Ratings.Add(rating);
        }

        void IRatingRepository.DeleteAllRatings()
        {
            var allRatings = _context.Ratings.ToList(); // Load all ratings into memory.

            foreach (var rating in allRatings)
            {
                _context.Ratings.Remove(rating);
            }
        }

        public void DeleteRating(Rating rating)
        {
            _context.Ratings.Remove(rating);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

    }
}