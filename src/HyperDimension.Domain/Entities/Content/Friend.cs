using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Domain.Entities.Content;

public class Friend : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public int Order { get; set; }

    public DateTimeOffset AddedTime { get; set; } = DateTimeOffset.UtcNow;

    public User? User { get; set; }
}
