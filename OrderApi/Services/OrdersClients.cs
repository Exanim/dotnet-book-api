using Microsoft.Extensions.Options;
using OrderApi.Configurations;

namespace OrderApi.Services
{
    public class OrdersClients : IOrdersClients
    {
        private readonly IHttpClientWrapper _userHttpClient;
        private readonly IHttpClientWrapper _productHttpClient;

        public OrdersClients(IOptions<ApiConfiguration> apiConfiguration,
            IHttpClientWrapper userHttpClient, IHttpClientWrapper productHttpClient)
        {
            var config = apiConfiguration.Value;

            _userHttpClient = userHttpClient;
            _userHttpClient.SetBaseAdress(config.UserApiUrl);

            _productHttpClient = productHttpClient;
            _productHttpClient.SetBaseAdress(config.ProductApiUrl);
        }

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
