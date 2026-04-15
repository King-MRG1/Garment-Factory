using Microsoft.AspNetCore.Diagnostics;

namespace API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                KeyNotFoundException ex => (404, ex.Message),
                InvalidOperationException ex => (400, ex.Message),
                ArgumentException ex => (400, ex.Message),
                UnauthorizedAccessException ex => (401, ex.Message),
                _                            => (500, "Something went wrong")
            };

            if(statusCode == 500)
                _logger.LogError(exception, "An unhandled exception occurred.");
            else 
                _logger.LogError(exception.Message);

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(new
            {
                StatusCode = statusCode,
                Massage = message
            },cancellationToken);

            return true;
        }
    }
}
