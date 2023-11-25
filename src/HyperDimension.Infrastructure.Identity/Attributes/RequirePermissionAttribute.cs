using HyperDimension.Infrastructure.Identity.Enums;

namespace HyperDimension.Infrastructure.Identity.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RequirePermissionAttribute : Attribute
{
    public IReadOnlyList<string> Nodes { get; }

    public string Permission { get; }

    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
        Nodes = permission.Split(".").ToArray();
    }

    public RequirePermissionAttribute(SpecialPermission permission)
    {
        var permissionString = permission switch
        {
            SpecialPermission.Anonymous => "%",
            SpecialPermission.Authenticated => "-",
            SpecialPermission.Any => "*",
            _ => "?"
        };

        Permission = permissionString;
        Nodes = [permissionString];
    }
}
