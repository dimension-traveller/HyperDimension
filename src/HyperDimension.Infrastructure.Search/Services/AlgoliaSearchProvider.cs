using System.Linq.Expressions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Infrastructure.Search.Services;

public class AlgoliaSearchProvider : IHyperDimensionSearchService
{
    public Task<IReadOnlyCollection<TEntity>> MatchAsync<TDocument, TEntity>(Expression<Func<TDocument, string>> fieldSelector, string keyword, int start = 0, int size = 10) where TDocument : SearchableDocument<TEntity> where TEntity : BaseEntity
    {
        throw new NotImplementedException();
    }
}
