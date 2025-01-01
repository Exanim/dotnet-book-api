
using OrderApi.Error;

namespace OrderApi.Exceptions
{
    public class OrderException : CustomException
    {
        public OrderException(ErrorCode errorCode, string message) : base(errorCode, message)
        {
        }
    }
}
