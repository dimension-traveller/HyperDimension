using HyperDimension.Analyzer.Generator;
using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Content;

[HasSearchableProperty]
public class Note : BaseEntity
{
    [Searchable]
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    [Searchable]
    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
