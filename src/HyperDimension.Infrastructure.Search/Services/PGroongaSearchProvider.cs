using System.Linq.Expressions;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;
using HyperDimension.Infrastructure.Search.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Search.Services;

public class PGroongaSearchProvider : IHyperDimensionSearchService
{
    private readonly IHyperDimensionDbContext _dbContext;

    public PGroongaSearchProvider(IHyperDimensionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector,
        string keyword,
        int start = 0,
        int size = 10)
        where TDocument : SearchableDocument<TEntity>
        where TEntity : BaseEntity
    {
        const string methodName = nameof(PGroongaLinqExtensions.PGroongaMatch);
        return ExecuteQueryAsync<TDocument, TEntity>(fieldSelector, methodName, start, size, keyword);
    }

    private async Task<IReadOnlyCollection<TEntity>> ExecuteQueryAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector,
        string methodName,
        int start = 0,
        int size = 10,
        params object[] arguments)
        where TDocument : SearchableDocument<TEntity>
        where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var lambdaExpression = LambdaConstructor.BuildPgroongaExpression<TEntity>(propertyName, methodName, arguments);

        return await _dbContext.Set<TEntity>()
            .Where(lambdaExpression)
            .Skip(start)
            .Take(size)
            .ToListAsync();
    }
}
