namespace ReviewAPI.CustomErrors
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException(string? message, Exception? inner) : base(message, inner)
        {

        }
    }
}
