using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Enums;

namespace HyperDimension.Domain.Entities.Content;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public ProjectStatus Status { get; set; }

    public int Order { get; set; }

    public DateTimeOffset AddedTime { get; set; } = DateTimeOffset.UtcNow;
}
