namespace ReviewAPI.Services.Clients
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }
        public Task<HttpResponseMessage> GetAsync(string? requestUri)
        {
            return  _httpClient.GetAsync(requestUri);
        }

        public void setBaseAddress(String url)
        {
            _httpClient.BaseAddress = new Uri(url);
        }

    }
}
