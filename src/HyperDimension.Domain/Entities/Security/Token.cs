using HyperDimension.Domain.Entities.Common;
using HyperDimension.Domain.Enums;

namespace HyperDimension.Domain.Entities.Security;

public class Token : BaseEntity
{
    public string Value { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ExpiredAt { get; set; }

    public Guid BindTo { get; set; }

    public TokenUsage Usage { get; set; }
}
