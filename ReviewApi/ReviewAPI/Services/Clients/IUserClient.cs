using ReviewAPI.Services.Caching;

namespace ReviewAPI.Services.Clients
{
    public interface IUserClient
    {
        Task<bool> DoesUserExistAsync(int UserId);
    }
}
