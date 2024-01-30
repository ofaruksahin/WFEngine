using Microsoft.AspNetCore.Diagnostics;

namespace WFEngine.Presentation.AuthorizationServer.Middlewares
{
    internal class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger;
        }

        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Error,exception,exception.Message);
            return ValueTask.FromResult(false);
        }
    }
}
