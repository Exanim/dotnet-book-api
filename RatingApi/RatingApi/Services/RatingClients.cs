using Microsoft.Extensions.Options;
using RatingApi.Configurations;

namespace RatingApi.Services
{
    public class RatingClients : IRatingClients
    {
        private readonly HttpClient _userHttpClient;
        private readonly HttpClient _productHttpClient;

        public RatingClients(IOptions<ApiConfiguration> apiConfiguration)
        {
            var config = apiConfiguration.Value;

            _userHttpClient = new HttpClient()
            {
                BaseAddress = new Uri(config.UserApiUrl)
            };

            _productHttpClient = new HttpClient()
            {
                BaseAddress = new Uri(config.ProductApiUrl)
            };
        }

        // TODO: dependency injection
        #if DEBUG
        public RatingClients(HttpClient userHttpClient, HttpClient productHttpClient)
        {
            _userHttpClient = userHttpClient;
            _productHttpClient = productHttpClient;
        }
        #endif

        public async Task<HttpResponseMessage> GetUserAsync(int userId)
        {
            return await _userHttpClient.GetAsync($"/user/{userId}");
        }

        public async Task<HttpResponseMessage> GetProductAsync(int productId)
        {
            return await _productHttpClient.GetAsync($"/products/{productId}");
        }
    }
}