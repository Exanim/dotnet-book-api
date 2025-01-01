namespace RatingApi.Services
{
    public interface IRatingClients
    {
        Task<HttpResponseMessage> GetUserAsync(int userId);

        Task<HttpResponseMessage> GetProductAsync(int productId);
    }
}