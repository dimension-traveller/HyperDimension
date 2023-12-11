using System.Linq.Expressions;
using Elastic.Clients.Elasticsearch;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Search.Services;

public class ElasticSearchProvider : IHyperDimensionSearchService
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IDatabaseMetadataService _databaseMetadataService;
    private readonly ElasticsearchClient _es;

    public ElasticSearchProvider(
        IHyperDimensionDbContext dbContext,
        IDatabaseMetadataService databaseMetadataService,
        ElasticsearchClient es)
    {
        _dbContext = dbContext;
        _databaseMetadataService = databaseMetadataService;
        _es = es;
    }

    public async Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector, string keyword, int start = 0, int size = 10)
        where TDocument : SearchableDocument<TEntity>
        where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var tableName = await _databaseMetadataService.GetTableName<TEntity>();
        var columnName = await _databaseMetadataService.GetColumnName<TEntity>(propertyName);

        var response = await _es.SearchAsync<TDocument>(search =>
        {
            search
                .Index($"hd-{tableName}")
                .From(start)
                .Size(size)
                .Query(query => query.Match(x =>
                    x.Field(columnName).Query(keyword)));
        });

        var documents = response.Documents;
        var ids = documents.Select(x => x.EntityId).ToList();
        return await _dbContext.Set<TEntity>()
            .Where(x => ids.Contains(x.EntityId))
            .ToListAsync();
    }
}
