namespace EnglishAI.Application.Common.Models;

public sealed class ApiErrorResponse
{
    public ApiErrorResponse(string message)
    {
        Message = message;
    }

    public bool Success { get; init; } = false;
    public string Message { get; init; }
    public string? Code { get; init; }
    public object? Details { get; init; }
}

