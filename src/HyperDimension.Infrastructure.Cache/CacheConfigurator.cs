using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Cache.Enums;
using HyperDimension.Infrastructure.Cache.Exceptions;
using HyperDimension.Infrastructure.Cache.Options;
using HyperDimension.Infrastructure.Cache.Options.Providers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;
using StackExchange.Redis.Extensions.Core;

namespace HyperDimension.Infrastructure.Cache;

public static class CacheConfigurator
{
    public static void AddHyperDimensionCache(this IServiceCollection services)
    {
        var cacheOptions = HyperDimensionConfiguration.Instance.GetOption<CacheOptions>();

        switch (cacheOptions.Provider)
        {
            case CacheProvider.Memory:
                services.AddDistributedMemoryCache();
                break;
            case CacheProvider.Redis:
                services.AddSingleton<IRedisClientFactory, RedisClientFactory>();
                services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

                services.AddSingleton(sp => sp
                    .GetRequiredService<IRedisClientFactory>()
                    .GetDefaultRedisClient());
                services.AddSingleton(sp => sp
                    .GetRequiredService<IRedisClientFactory>()
                    .GetDefaultRedisClient()
                    .GetDefaultDatabase());

                services.AddSingleton<IEnumerable<RedisConfiguration>>(sp =>
                {
                    var redisOptions = sp.GetRequiredService<RedisOptions>();

                    return
                    [
                        new RedisConfiguration
                        {
                            ConnectionString = redisOptions.ConnectionString
                        }
                    ];
                });

                services.AddSingleton<IOptions<RedisCacheOptions>>(sp =>
                    new OptionsWrapper<RedisCacheOptions>(new RedisCacheOptions
                    {
                        ConnectionMultiplexerFactory = () =>
                            Task.FromResult(sp.GetRequiredService<IRedisClient>().ConnectionPoolManager.GetConnection())
                    }));

                services.AddSingleton<IDistributedCache, RedisCache>(sp =>
                    new RedisCache(sp.GetRequiredService<IOptions<RedisCacheOptions>>()));
                break;
            default:
                throw new CacheNotSupportedException(cacheOptions.Provider.ToString(), "Unknown cache provider.");
        }
    }
}
