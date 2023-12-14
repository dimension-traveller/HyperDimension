using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Distributed;

namespace HyperDimension.Infrastructure.Database.Services;

public class DatabaseMetadataService : IDatabaseMetadataService
{
    private readonly IDistributedCache _cache;
    private readonly IModel _dbModel;

    public DatabaseMetadataService(HyperDimensionDbContext dbContext, IDistributedCache cache)
    {
        _cache = cache;
        _dbModel = dbContext.Model;
    }

    public async Task<string> GetTableName<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity);
        var typeName = type.FullName ?? type.Name;
        var cacheKey = $"db:table:{typeName}";

        var cachedTableName = await _cache.GetStringAsync(cacheKey);
        if (cachedTableName is not null)
        {
            return cachedTableName;
        }

        var entityType = _dbModel.FindEntityType(type).ExpectNotNull();
        var tableName = entityType.GetTableName().ExpectNotNull();

        await _cache.SetStringAsync(cacheKey, tableName, 8 * 3600);
        return tableName;
    }

    public async Task<string> GetColumnName<TEntity>(string propertyName) where TEntity : BaseEntity
    {
        var type = typeof(TEntity);
        var typeName = type.FullName ?? type.Name;
        var cacheKey = $"db:column:{typeName}-{propertyName}";

        var cachedColumnName = await _cache.GetStringAsync(cacheKey);
        if (cachedColumnName is not null)
        {
            return cachedColumnName;
        }

        var entityType = _dbModel.FindEntityType(type).ExpectNotNull();
        var property = entityType.FindProperty(propertyName).ExpectNotNull();
        var columnName = property.GetColumnName().ExpectNotNull();

        await _cache.SetStringAsync(cacheKey, columnName, 8 * 3600);
        return columnName;
    }
}
