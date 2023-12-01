using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Common.Utilities;
using Microsoft.Extensions.Caching.Distributed;

namespace HyperDimension.Infrastructure.Identity.Services;

public class HyperDimensionIdentityService
    : IHyperDimensionIdentityService
{
    private readonly IDistributedCache _cache;

    public HyperDimensionIdentityService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<Result<Guid>> VerifyLoginTokenAsync(string token)
    {
        var userId = await _cache.GetStringAsync(token);
        if (userId is null)
        {
            return "Token invalid or expired.";
        }
        await _cache.RemoveAsync(token);
        return Guid.Parse(userId);
    }

    public async Task<string> CreateLoginTokenAsync(Guid userId)
    {
        var token = RandomUtility.GenerateToken(32);
        await _cache.SetStringAsync(token, userId.ToString());
        return token;
    }
}
