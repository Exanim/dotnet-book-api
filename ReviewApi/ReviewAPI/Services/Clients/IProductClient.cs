using ReviewAPI.Services.Caching;

namespace ReviewAPI.Services.Clients
{
    public interface IProductClient
    {
        Task<bool> DoesProductExistAsync(int ProductId);
    }
}
