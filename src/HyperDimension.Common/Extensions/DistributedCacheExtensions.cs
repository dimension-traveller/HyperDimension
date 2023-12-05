using Microsoft.Extensions.Caching.Distributed;

namespace HyperDimension.Common.Extensions;

public static class DistributedCacheExtensions
{
    public static Task SetStringAsync(this IDistributedCache cache, string key, string value, uint expire)
    {
        return cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expire)
        });
    }
}
