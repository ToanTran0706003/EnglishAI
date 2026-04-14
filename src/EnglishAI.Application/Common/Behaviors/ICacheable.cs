namespace EnglishAI.Application.Common.Behaviors;

public interface ICacheable
{
    string CacheKey { get; }
    int CacheDurationMinutes { get; }
}

