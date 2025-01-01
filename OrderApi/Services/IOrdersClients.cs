namespace OrderApi.Services
{
    public interface IOrdersClients
    {
        Task<HttpResponseMessage> GetUserAsync(int userId);

        Task<HttpResponseMessage> GetProductAsync(int productId);
    }
}
