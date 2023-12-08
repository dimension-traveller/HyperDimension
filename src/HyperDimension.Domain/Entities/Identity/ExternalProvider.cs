using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class ExternalProvider : BaseEntity
{
    public string ProviderId { get; set; } = string.Empty;

    public string UserIdentifier { get; set; } = string.Empty;
}
