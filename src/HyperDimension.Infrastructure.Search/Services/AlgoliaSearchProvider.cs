using System.Linq.Expressions;
using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;
using HyperDimension.Infrastructure.Search.Options;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Search.Services;

public class AlgoliaSearchProvider : IHyperDimensionSearchService
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IDatabaseMetadataService _databaseMetadataService;
    private readonly ISearchClient _client;
    private readonly string _indexPrefix;

    public AlgoliaSearchProvider(
        IHyperDimensionDbContext dbContext,
        IDatabaseMetadataService databaseMetadataService,
        SearchOptions searchOptions,
        ISearchClient client)
    {
        _dbContext = dbContext;
        _databaseMetadataService = databaseMetadataService;
        _client = client;

        _indexPrefix = searchOptions.Algolia.ExpectNotNull().IndexPrefix;
    }

    public async Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector,
        string keyword, int start = 0, int size = 10)
        where TDocument : SearchableDocument
        where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var columnName = await _databaseMetadataService.GetColumnName<TEntity>(propertyName);

        var query = new Query(keyword)
        {
            Offset = start,
            Length = size,
            RestrictSearchableAttributes = [columnName]
        };

        return await ExecuteSearchAsync<TDocument, TEntity>(query);
    }

    private async Task<IReadOnlyCollection<TEntity>> ExecuteSearchAsync<TDocument, TEntity>(Query query)
        where TDocument : SearchableDocument
        where TEntity : BaseEntity
    {
        var indexName = await GetIndexName<TEntity>();

        var index = _client.InitIndex(indexName);

        var result = await index.SearchAsync<TDocument>(query);
        if (result is null)
        {
            return [];
        }

        var ids = result.Hits.Select(x => x.EntityId);
        return await _dbContext.Set<TEntity>()
            .Where(x => ids.Contains(x.EntityId))
            .ToListAsync();
    }

    private async Task<string> GetIndexName<TEntity>() where TEntity : BaseEntity
    {
        var tableName = await _databaseMetadataService.GetTableName<TEntity>();
        return $"{_indexPrefix}-{tableName}";
    }
}
