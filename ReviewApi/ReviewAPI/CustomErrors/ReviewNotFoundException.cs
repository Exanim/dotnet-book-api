namespace ReviewAPI.CustomErrors
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(string? message, Exception? inner) : base(message, inner)
        {

        }
    }
}
