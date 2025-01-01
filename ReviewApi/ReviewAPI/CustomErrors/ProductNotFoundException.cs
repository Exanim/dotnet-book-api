namespace ReviewAPI.CustomErrors
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string? message, Exception? inner) : base(message, inner)
        {

        }
    }
}
