namespace OrderApi.Services
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
        void SetBaseAdress(string adress);
    }
}
