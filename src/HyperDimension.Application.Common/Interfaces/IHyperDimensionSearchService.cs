using System.Linq.Expressions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionSearchService
{
    public Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(
        Expression<Func<TDocument, string>> fieldSelector, string keyword, int start = 0, int size = 10)
        where TEntity : BaseEntity
        where TDocument : SearchableDocument<TEntity>;
}
