using EnglishAI.Application.Common.Exceptions;
using EnglishAI.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnglishAI.API.Filters;

public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var (statusCode, response) = context.Exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, new ApiErrorResponse(context.Exception.Message) { Code = "not_found" }),
            EnglishAI.Application.Common.Exceptions.ValidationException vex =>
                (StatusCodes.Status400BadRequest, new ApiErrorResponse(vex.Message) { Code = "validation_error", Details = vex.Errors }),
            ForbiddenException => (StatusCodes.Status403Forbidden, new ApiErrorResponse(context.Exception.Message) { Code = "forbidden" }),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, new ApiErrorResponse(context.Exception.Message) { Code = "unauthorized" }),
            RateLimitException => (StatusCodes.Status429TooManyRequests, new ApiErrorResponse(context.Exception.Message) { Code = "rate_limited" }),
            _ => (StatusCodes.Status500InternalServerError, new ApiErrorResponse("Internal server error.") { Code = "server_error" }),
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode,
        };

        context.ExceptionHandled = true;
    }
}

