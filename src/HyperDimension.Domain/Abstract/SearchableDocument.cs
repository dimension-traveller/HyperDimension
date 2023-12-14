using System.Diagnostics.CodeAnalysis;

namespace HyperDimension.Domain.Abstract;

[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed")]
public class SearchableDocument
{
    public Guid EntityId { get; set; }
}
