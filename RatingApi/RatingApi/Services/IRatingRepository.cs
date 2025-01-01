using RatingApi.Entities;

namespace RatingApi.Services
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetRatings(int? userId, int? productId);
        Task<Rating> GetRatingById(int id);
        void AddRating(Rating review);
        void DeleteAllRatings();
        void DeleteRating(Rating review);
        Task<bool> SaveChangesAsync();
    }
}