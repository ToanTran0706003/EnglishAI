using System.Net;
using EnglishAI.Application.Common.Exceptions;
using EnglishAI.Application.Common.Models;

namespace EnglishAI.API.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception");

        var (statusCode, response) = ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, new ApiErrorResponse(ex.Message) { Code = "not_found" }),
            EnglishAI.Application.Common.Exceptions.ValidationException vex =>
                (HttpStatusCode.BadRequest, new ApiErrorResponse(vex.Message) { Code = "validation_error", Details = vex.Errors }),
            ForbiddenException => (HttpStatusCode.Forbidden, new ApiErrorResponse(ex.Message) { Code = "forbidden" }),
            UnauthorizedException => (HttpStatusCode.Unauthorized, new ApiErrorResponse(ex.Message) { Code = "unauthorized" }),
            RateLimitException => (HttpStatusCode.TooManyRequests, new ApiErrorResponse(ex.Message) { Code = "rate_limited" }),
            _ => (HttpStatusCode.InternalServerError, new ApiErrorResponse("Internal server error.") { Code = "server_error" }),
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}

