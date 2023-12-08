using System.ComponentModel.DataAnnotations.Schema;
using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class TotpRecoveryCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UsedAt { get; set; }

    [NotMapped]
    public bool IsUsed => UsedAt.HasValue;
}
