using Microsoft.Extensions.Options;
using ReviewAPI.Configurations;
using ReviewAPI.Controllers;
using ReviewAPI.Services.Caching;

namespace ReviewAPI.Services.Clients
{
    public class UserClient : IUserClient
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ICache _clientsCache;
        private readonly ILogger<UserClient> _logger;

        public UserClient(
            IHttpClientWrapper httpClient,
            IOptions<ApiConfiguration> apiConfiguration, 
            ICache clientsCache,
            ILogger<UserClient> logger)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.setBaseAddress(apiConfiguration.Value.UserApiUrl);
            _clientsCache = clientsCache 
                ?? throw new ArgumentNullException(nameof(clientsCache));
            _logger = logger
                ??throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DoesUserExistAsync(int userId)
        {
            if (_clientsCache.IsCached(CacheType.User, userId))
            {
                _logger.LogDebug("User FOUND in cache");
                return true;
            }

            _logger.LogDebug("User NOT found in cache");
            var serverResponse = await _httpClient.GetAsync($"/user/{userId}");

            if (serverResponse.IsSuccessStatusCode)
            {
                _clientsCache.Add(CacheType.User, userId, true);
                return true;
            }
            else
            {
                _clientsCache.Add(CacheType.User, userId, false);
                return false;
            }
        }
    }
}
