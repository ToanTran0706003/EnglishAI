namespace EnglishAI.Application.Common.Models;

public sealed class ApiResponse<T>
{
    public ApiResponse(T data)
    {
        Data = data;
    }

    public bool Success { get; init; } = true;
    public T Data { get; init; }
}

