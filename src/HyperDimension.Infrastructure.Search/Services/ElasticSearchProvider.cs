using System.Linq.Expressions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;
using HyperDimension.Infrastructure.Search.Options;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Search.Services;

public class ElasticSearchProvider : IHyperDimensionSearchService
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IDatabaseMetadataService _databaseMetadataService;
    private readonly ElasticsearchClient _es;
    private readonly string _indexPrefix;

    public ElasticSearchProvider(
        IHyperDimensionDbContext dbContext,
        IDatabaseMetadataService databaseMetadataService,
        SearchOptions searchOptions,
        ElasticsearchClient es)
    {
        _dbContext = dbContext;
        _databaseMetadataService = databaseMetadataService;
        _es = es;

        _indexPrefix = searchOptions.ElasticSearch.ExpectNotNull().IndexPrefix;
    }

    public async Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector,
        string keyword,
        int start = 0,
        int size = 10)
        where TDocument : SearchableDocument<TEntity>
        where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var columnName = await _databaseMetadataService.GetColumnName<TEntity>(propertyName);

        var matchQuery = new QueryDescriptor<TDocument>()
            .Match(m => m.Field(columnName).Query(keyword));

        return await ExecuteQueryAsync<TDocument, TEntity>(matchQuery, start, size);
    }

    private async Task<IReadOnlyCollection<TEntity>> ExecuteQueryAsync<TDocument, TEntity>(
        QueryDescriptor<TDocument> queryDescriptor,
        int start = 0,
        int size = 10)
        where TDocument : SearchableDocument<TEntity>
        where TEntity : BaseEntity
    {
        var indexName = await GetIndexName<TEntity>();

        var response = await _es.SearchAsync<TDocument>(search =>
        {
            search.Index(indexName)
                .From(start)
                .Size(size)
                .Query(queryDescriptor);
        });

        var documents = response.Documents;
        var ids = documents.Select(x => x.EntityId).ToList();
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
