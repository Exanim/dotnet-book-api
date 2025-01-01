namespace ReviewAPI.Services.Clients
{
    public interface IHttpClientWrapper
    {
        void setBaseAddress(string url);
        Task<HttpResponseMessage> GetAsync(string? requestUri);

    }
}
