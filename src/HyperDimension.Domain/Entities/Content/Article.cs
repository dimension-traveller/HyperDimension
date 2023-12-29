using HyperDimension.Analyzer.Generator;
using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Entities.Organization;

namespace HyperDimension.Domain.Entities.Content;

[HasSearchableProperty]
public class Article : BaseEntity
{
    [Searchable]
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    [Searchable]
    public string Content { get; set; } = string.Empty;

    [Searchable]
    public string Summary { get; set; } = string.Empty;

    public string CoverImage { get; set; } = string.Empty;

    public int WordCount { get; set; }

    public int EstimatedReadingTime { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Category Category { get; set; } = null!;

    [Searchable]
    public List<string> Tags { get; set; } = [];
}
