using System.Text.Json;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace HyperDimension.Infrastructure.Identity.Services;

public class PermissionService : IPermissionService
{
    private readonly IDistributedCache _cache;
    private readonly IHyperDimensionDbContext _dbContext;

    public PermissionService(IDistributedCache cache, IHyperDimensionDbContext dbContext)
    {
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<bool> AllowAccess(string permission, Guid userId)
    {
        var userPermissionCheckCacheKey = $"user:permission:{userId}:{permission}";

        // Get user permission check result from cache
        var userPermissionCheckResultString = await _cache.GetStringAsync(userPermissionCheckCacheKey);
        if (userPermissionCheckResultString is not null)
        {
            return bool.Parse(userPermissionCheckResultString);
        }

        var userPermissionCacheKey = $"user:permission:{userId}";

        // Get user permission from cache
        var userPermissionString = await _cache.GetStringAsync(userPermissionCacheKey);

        bool checkResult;

        // Cache hit
        if (userPermissionString is not null)
        {
            var userPermissions = JsonSerializer.Deserialize<List<string>>(userPermissionString)
                ?? [];
            checkResult = PermissionCheck(permission, userPermissions);
        }
        else
        {
            // Get from database
            var user = _dbContext.Users
                .Include(x => x.Roles)
                .First(x => x.EntityId == userId);

            var userPermissionsFromDatabase = user.Roles
                .SelectMany(x => x.Permissions)
                .Distinct()
                .ToList();

            // Cache user permission
            await _cache.SetStringAsync(userPermissionCacheKey, JsonSerializer.Serialize(userPermissionsFromDatabase), 3600 * 24);
            checkResult = PermissionCheck(permission, userPermissionsFromDatabase);
        }

        await _cache.SetStringAsync($"user:permission:{userId}:{permission}", checkResult.ToString(), 3600 * 24);
        return checkResult;
    }

    private static bool PermissionCheck(string permission, IReadOnlyCollection<string> userPermissions)
    {
        // User do not have permission
        if (userPermissions.Count == 0)
        {
            return false;
        }

        // User have exact permission
        if (userPermissions.Contains(permission))
        {
            return true;
        }

        // User have wildcard permission which allows all access
        if (userPermissions.Contains("*"))
        {
            return true;
        }

        // Split permission into segments
        var permissionSegment = permission.Split(".");

        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 1; i <= permissionSegment.Length; i++)
        {
            var wildcardPermission = string.Join(".", permissionSegment.Take(i)) + ".*";
            if (userPermissions.Contains(wildcardPermission))
            {
                return true;
            }
        }

        return false;
    }
}
