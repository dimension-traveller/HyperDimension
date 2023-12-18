using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class ApiToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? LastUsedAt { get; set; }

    public DateTimeOffset? ExpiredAt { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public User User { get; set; } = null!;
}
