namespace HyperDimension.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PermissionAttribute : Attribute
{
    public string Permission { get; }

    public PermissionAttribute(string permission)
    {
        Permission = permission;
    }
}
