namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// Generic cache abstraction.
/// </summary>
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct);

    Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct);

    Task RemoveAsync(string key, CancellationToken ct);

    Task<bool> ExistsAsync(string key, CancellationToken ct);

    Task<long> IncrementAsync(string key, long by, TimeSpan? ttl, CancellationToken ct);
}

