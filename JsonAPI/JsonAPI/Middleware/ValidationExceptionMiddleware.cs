using Application.Exceptions;

namespace JsonAPI.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;
        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access: missing or invalid user identity");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Not found: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ForbiddenException ex)
            {
                _logger.LogWarning("Forbidden:{Message}", ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError(ex, "Extetnal request failed: {Message}", ex.InnerException?.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status502BadGateway;

                await context.Response.WriteAsJsonAsync(new { error = "A downstream data service error occurred", details = ex.InnerException?.Message });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });
            }
        }
    }
}
