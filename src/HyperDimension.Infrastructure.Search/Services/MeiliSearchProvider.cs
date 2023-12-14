using System.Linq.Expressions;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;
using HyperDimension.Infrastructure.Search.Options;
using Meilisearch;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Search.Services;

public class MeiliSearchProvider : IHyperDimensionSearchService
{
    private readonly IDatabaseMetadataService _databaseMetadataService;
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly MeilisearchClient _client;
    private readonly string _indexPrefix;

    public MeiliSearchProvider(
        IDatabaseMetadataService databaseMetadataService,
        SearchOptions searchOptions,
        IHyperDimensionDbContext dbContext,
        MeilisearchClient client)
    {
        _databaseMetadataService = databaseMetadataService;
        _dbContext = dbContext;
        _client = client;

        _indexPrefix = searchOptions.MeiliSearch.ExpectNotNull().IndexPrefix;
    }

    public async Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector,
        string keyword, int start = 0, int size = 10)
        where TDocument : SearchableDocument
        where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var columnName = await _databaseMetadataService.GetColumnName<TEntity>(propertyName);

        var query = new SearchQuery
        {
            Limit = size,
            Offset = start,
            AttributesToRetrieve = new[] { columnName }
        };

        return await ExecuteQueryAsync<TDocument, TEntity>(keyword, query);
    }

    private async Task<IReadOnlyCollection<TEntity>> ExecuteQueryAsync<TDocument, TEntity>(string q, SearchQuery searchQuery)
        where TDocument : SearchableDocument
        where TEntity : BaseEntity
    {
        var indexName = await GetIndexName<TEntity>();

        var index = _client.Index(indexName);
        var searchResult = await index.SearchAsync<TDocument>(q, searchQuery);
        if (searchResult is null)
        {
            return [];
        }

        var ids = searchResult.Hits.Select(x => x.EntityId);
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
