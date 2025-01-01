using ReviewAPI.Entities;
namespace ReviewAPI.Services
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsAsync(int? userId, int? productId);
        Task<Review> GetReviewByIdAsync(int ReviewId);
        void AddReview(Review review);
        void DeleteReview(Review review);
        Task<bool> SaveChangesAsync();
    }
}
