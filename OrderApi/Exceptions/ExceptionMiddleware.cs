using Newtonsoft.Json;
using OrderApi.Error;

namespace OrderApi.Exceptions
{
    public class ExceptionMiddleware<TException> where TException : CustomException
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (TException ex)
            {
                await HandleException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            ErrorCode errorCode;
            string errorMessage;

            if (exception is TException customException)
            {
                errorCode = customException.ErrorCode;
                errorMessage = customException.Message;
            }
            else
            {
                Console.WriteLine($"Exception: {exception.Message}");
                errorCode = ErrorCode.InternalServerError;
                errorMessage = "Internal server error.";
            }

            context.Response.StatusCode = GetStatusCode(errorCode);
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };

            var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(jsonErrorResponse);
        }

        private int GetStatusCode(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.InternalServerError:
                    return 500;
                case ErrorCode.UserNotFound:
                case ErrorCode.ProductNotFound:
                case ErrorCode.OrderNotFound:
                    return 404;
                default:
                    return 500;
            }
        }
    }
}
