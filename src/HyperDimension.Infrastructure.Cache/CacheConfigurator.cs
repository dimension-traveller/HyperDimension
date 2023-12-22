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
using OpenTelemetry.Trace;
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

        switch (cacheOptions.Type)
        {
            case CacheType.Memory:
                var memoryOptions = HyperDimensionConfiguration.Instance.GetOption<MemoryOptions>();
                services.AddDistributedMemoryCache(options =>
                {
                    options.SizeLimit = memoryOptions.SizeLimit * 1024 * 1024;
                });
                break;
            case CacheType.Redis:
                var redisOptions = HyperDimensionConfiguration.Instance.GetOption<RedisOptions>();
                services.AddSingleton<IRedisClientFactory, RedisClientFactory>();
                services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

                services.AddSingleton(sp => sp
                    .GetRequiredService<IRedisClientFactory>()
                    .GetDefaultRedisClient());
                services.AddSingleton(sp => sp
                    .GetRequiredService<IRedisClientFactory>()
                    .GetDefaultRedisClient()
                    .GetDb(redisOptions.Database, redisOptions.KeyPrefix));

                services.AddSingleton<IEnumerable<RedisConfiguration>>(_ =>
                [
                    new RedisConfiguration
                    {
                        ConnectionString = redisOptions.ConnectionString,
                        Database = redisOptions.Database,
                        KeyPrefix = redisOptions.KeyPrefix
                    }
                ]);

                services.AddSingleton<IOptions<RedisCacheOptions>>(sp =>
                    new OptionsWrapper<RedisCacheOptions>(new RedisCacheOptions
                    {
                        ConnectionMultiplexerFactory = () =>
                            Task.FromResult(sp.GetRequiredService<IRedisClient>().ConnectionPoolManager.GetConnection())
                    }));

                services.AddSingleton<IDistributedCache, RedisCache>(sp =>
                    new RedisCache(sp.GetRequiredService<IOptions<RedisCacheOptions>>()));

                services.AddHealthChecks().AddRedis(sp =>
                    sp.GetRequiredService<IRedisClient>().ConnectionPoolManager.GetConnection(), "redis");

                if (redisOptions.Tracing)
                {
                    services.ConfigureOpenTelemetryTracerProvider(builder =>
                    {
                        builder.AddRedisInstrumentation(options =>
                        {
                            options.SetVerboseDatabaseStatements = true;
                            options.EnrichActivityWithTimingEvents = true;
                        });
                        builder.ConfigureRedisInstrumentation((sp, instrumentation) =>
                        {
                            var connections = sp.GetRequiredService<IRedisClient>()
                                .ConnectionPoolManager
                                .GetConnections();

                            foreach (var connection in connections)
                            {
                                instrumentation.AddConnection(connection);
                            }
                        });
                    });
                }
                break;
            default:
                throw new CacheNotSupportedException(cacheOptions.Type.ToString(), "Unknown cache provider.");
        }
    }
}
