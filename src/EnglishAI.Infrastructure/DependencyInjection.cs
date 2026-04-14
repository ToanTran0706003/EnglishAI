using EnglishAI.Infrastructure.Caching;
using EnglishAI.Application.Common.Interfaces;
using EnglishAI.Infrastructure.Persistence;
using EnglishAI.Infrastructure.Persistence.Interceptors;
using EnglishAI.Infrastructure.Services.Auth;
using EnglishAI.Infrastructure.Services.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EnglishAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? configuration["ConnectionStrings:DefaultConnection"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");
            }

            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();

            options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        });

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        var redisConnectionString =
            configuration.GetConnectionString("Redis")
            ?? configuration["ConnectionStrings:Redis"]
            ?? configuration["Redis:ConnectionString"];

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<JwtService>();
        services.AddSingleton<IFileStorageService, MinioStorageService>();

        return services;
    }
}

