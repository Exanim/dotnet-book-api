namespace ReviewAPI.CustomErrors
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string? message, Exception? inner) : base(message, inner)
        {

        }
    }
}
