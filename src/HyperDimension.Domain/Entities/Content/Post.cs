using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Domain.Entities.Content;

public class Post : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string CoverImage { get; set; } = string.Empty;

    public int WordCount { get; set; }

    public int EstimatedReadingTime { get; set; }

    public User Author { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
