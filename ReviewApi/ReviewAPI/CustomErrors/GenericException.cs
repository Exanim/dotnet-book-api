namespace ReviewAPI.CustomErrors
{
    public class GenericException : Exception
    {
        public GenericException(string? message, Exception? inner) : base(message, inner)
        {
        }
    }
}
