namespace EnglishAI.Application.Common.Exceptions;

public class RateLimitException : Exception
{
    public RateLimitException(string message = "Rate limit exceeded.")
        : base(message)
    {
    }
}

