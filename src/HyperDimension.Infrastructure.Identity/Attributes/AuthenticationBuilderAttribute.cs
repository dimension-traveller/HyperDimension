namespace HyperDimension.Infrastructure.Identity.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuthenticationBuilderAttribute(string name) : Attribute
{
    public string Name => name;
}
