using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Travelo.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleUnauthorizedAccessExceptionAsync(context, ex);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
            catch (ArgumentException ex)
            {
                await HandleBadRequestExceptionAsync(context, ex);
            }
            catch (InvalidOperationException ex)
            {
                await HandleInvalidOperationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnhandledExceptionAsync(context, ex);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogWarning(ex,
                "Validation error occurred. TraceId: {TraceId}, Path: {Path}",
                traceId,
                context.Request.Path);

            var errors = new Dictionary<string, string[]>
                {
                    { "Error", new[] { ex.ValidationResult?.ErrorMessage ?? ex.Message } }
                };


            await WriteResponseAsync(context, HttpStatusCode.BadRequest, new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Validation Error",
                status = (int)HttpStatusCode.BadRequest,
                traceId,
                errors
            });
        }

        private async Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogWarning(ex,
                "Unauthorized access attempt. TraceId: {TraceId}, Path: {Path}, User: {User}",
                traceId,
                context.Request.Path,
                context.User?.Identity?.Name ?? "Anonymous");

            await WriteResponseAsync(context, HttpStatusCode.Unauthorized, new
            {
                type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                title = "Unauthorized",
                status = (int)HttpStatusCode.Unauthorized,
                detail = "You are not authorized to access this resource. Please authenticate and try again.",
                traceId
            });
        }

        private async Task HandleNotFoundExceptionAsync(HttpContext context, KeyNotFoundException ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogWarning(ex,
                "Resource not found. TraceId: {TraceId}, Path: {Path}",
                traceId,
                context.Request.Path);

            await WriteResponseAsync(context, HttpStatusCode.NotFound, new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                title = "Resource Not Found",
                status = (int)HttpStatusCode.NotFound,
                detail = ex.Message,
                traceId
            });
        }

        private async Task HandleBadRequestExceptionAsync(HttpContext context, ArgumentException ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogWarning(ex,
                "Bad request. TraceId: {TraceId}, Path: {Path}",
                traceId,
                context.Request.Path);

            await WriteResponseAsync(context, HttpStatusCode.BadRequest, new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Bad Request",
                status = (int)HttpStatusCode.BadRequest,
                detail = ex.Message,
                traceId
            });
        }

        private async Task HandleInvalidOperationExceptionAsync(HttpContext context, InvalidOperationException ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogWarning(ex,
                "Invalid operation. TraceId: {TraceId}, Path: {Path}",
                traceId,
                context.Request.Path);

            await WriteResponseAsync(context, HttpStatusCode.BadRequest, new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Invalid Operation",
                status = (int)HttpStatusCode.BadRequest,
                detail = ex.Message,
                traceId
            });
        }

        private async Task HandleUnhandledExceptionAsync(HttpContext context, Exception ex)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(ex,
                "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
                traceId,
                context.Request.Path,
                context.Request.Method);

            var detail = _environment.IsDevelopment()
                ? $"{ex.Message}\n\nStack Trace:\n{ex.StackTrace}"
                : "An unexpected error occurred. Please contact support if the problem persists.";

            await WriteResponseAsync(context, HttpStatusCode.InternalServerError, new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title = "Internal Server Error",
                status = (int)HttpStatusCode.InternalServerError,
                detail,
                traceId
            });
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, object errorResponse)
        {
            // Check if response has already started
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/problem+json";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await context.Response.WriteAsJsonAsync(errorResponse, options);
        }
    }
}
