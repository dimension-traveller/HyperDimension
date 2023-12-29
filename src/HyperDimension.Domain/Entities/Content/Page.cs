using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Enums;

namespace HyperDimension.Domain.Entities.Content;

public class Page : BaseEntity
{
    public PageType PageType { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
