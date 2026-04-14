using System.Text.Json;
using EnglishAI.Application.Common.Interfaces;
using StackExchange.Redis;

namespace EnglishAI.Infrastructure.Caching;

public sealed class RedisCacheService : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer mux)
    {
        _db = mux.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        var value = await _db.StringGetAsync(key);
        if (!value.HasValue)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(value!, JsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await _db.StringSetAsync(key, json, ttl);
    }

    public Task RemoveAsync(string key, CancellationToken ct) => _db.KeyDeleteAsync(key);

    public Task<bool> ExistsAsync(string key, CancellationToken ct) => _db.KeyExistsAsync(key);

    public async Task<long> IncrementAsync(string key, long by, TimeSpan? ttl, CancellationToken ct)
    {
        var value = await _db.StringIncrementAsync(key, by);
        if (ttl is not null)
        {
            await _db.KeyExpireAsync(key, ttl);
        }

        return value;
    }
}

