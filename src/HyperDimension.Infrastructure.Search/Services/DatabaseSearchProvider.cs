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

public class DatabaseSearchProvider : IHyperDimensionSearchService
{
    private readonly IHyperDimensionDbContext _dbContext;

    public DatabaseSearchProvider(
        IHyperDimensionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector,
        string keyword, int start = 0, int size = 10)
        where TDocument : SearchableDocument
        where TEntity : BaseEntity
    {
        var propertyName = fieldSelector.GetPropertyInfo().ExpectNotNull().Name;
        var method = typeof(string).GetMethod("Contains", [typeof(string)]).ExpectNotNull();

        var expression = LambdaConstructor.BuildStringExpression<TEntity>(propertyName, method, keyword);
        return await ExecuteSearchAsync(expression, start, size);
    }

    private async Task<IReadOnlyCollection<TEntity>> ExecuteSearchAsync<TEntity>(
        Expression<Func<TEntity, bool>> expression, int start, int size)
        where TEntity : BaseEntity
    {
        return await _dbContext.Set<TEntity>()
            .Where(expression)
            .Skip(start)
            .Take(size)
            .ToListAsync();
    }
}
