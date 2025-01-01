using Microsoft.Extensions.Options;
using ReviewAPI.Configurations;
using ReviewAPI.Services.Caching;

namespace ReviewAPI.Services.Clients
{
    public class ProductClient : IProductClient
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ICache _clientsCache;
        private readonly ILogger<ProductClient> _logger;

        public ProductClient(
            IHttpClientWrapper httpClient,
            IOptions<ApiConfiguration> apiConfiguration, 
            ICache clientsCache,
            ILogger<ProductClient> logger)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.setBaseAddress(apiConfiguration.Value.ProductApiUrl);
            _clientsCache = clientsCache
                ?? throw new ArgumentNullException(nameof(clientsCache));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DoesProductExistAsync(int productId)
        {
            if (_clientsCache.IsCached(CacheType.Product, productId))
            {
                _logger.LogDebug("Product FOUND in cache");
                return true;
            }
            _logger.LogDebug("User NOT found in cache");
            var serverResponse = await _httpClient.GetAsync($"/products/{productId}");

            if (serverResponse.IsSuccessStatusCode)
            {
                _clientsCache.Add(CacheType.Product, productId, true);
                return true;
            }
            else
            {
                _clientsCache.Add(CacheType.Product, productId, false);
                return false;
            }
        }
    }
}
