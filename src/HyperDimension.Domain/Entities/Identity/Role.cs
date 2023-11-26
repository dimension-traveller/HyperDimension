using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;

    public List<string> Permissions { get; set; } = [];
}
