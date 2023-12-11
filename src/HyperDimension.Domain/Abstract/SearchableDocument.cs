using System.Diagnostics.CodeAnalysis;
using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Abstract;

[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed")]
// ReSharper disable once UnusedTypeParameter
public class SearchableDocument<T> where T : BaseEntity
{
    public Guid EntityId { get; set; }
}
