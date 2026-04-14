using EnglishAI.Application.Common.Interfaces;
using MediatR;

namespace EnglishAI.Application.Common.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cache;

    public CachingBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheable cacheable)
        {
            return await next();
        }

        var cached = await _cache.GetAsync<TResponse>(cacheable.CacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var response = await next();

        await _cache.SetAsync(
            cacheable.CacheKey,
            response,
            TimeSpan.FromMinutes(cacheable.CacheDurationMinutes),
            cancellationToken);

        return response;
    }
}

