using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HyperDimension.Infrastructure.Search.Services;

public class PGroongaSearchProvider : IHyperDimensionSearchService
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IDatabaseMetadataService _databaseMetadataService;

    public PGroongaSearchProvider(IHyperDimensionDbContext dbContext, IDatabaseMetadataService databaseMetadataService)
    {
        _dbContext = dbContext;
        _databaseMetadataService = databaseMetadataService;
    }

    public async Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(Expression<Func<TDocument, string>> fieldSelector, string keyword, int start = 0, int size = 10) where TDocument : SearchableDocument<TEntity> where TEntity : BaseEntity
    {
        return await PGroongaSearchAsync<TDocument, TEntity>(fieldSelector, "&@", keyword);
    }

    [SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.")]
    private async Task<List<TEntity>> PGroongaSearchAsync<TDocument, TEntity>(Expression<Func<TDocument, string>> fieldSelector, string @operator, string expression) where TDocument : SearchableDocument<TEntity> where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var tableName = await _databaseMetadataService.GetTableName<TEntity>();
        var columnName = await _databaseMetadataService.GetColumnName<TEntity>(propertyName);

        var query = new NpgsqlParameter("query", expression);

        var data = await _dbContext.Set<TEntity>()
            .FromSqlRaw($"""
                         SELECT * FROM public."{tableName}" WHERE "{columnName}" {@operator} @query
                         """, query)
            .AsNoTracking()
            .ToListAsync();

        return data;
    }
}
