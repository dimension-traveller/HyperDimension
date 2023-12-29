using System.Diagnostics.CodeAnalysis;
using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Entities.Content;

namespace HyperDimension.Domain.Entities.Organization;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class Collection : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<Article> Articles { get; set; } = [];
}
