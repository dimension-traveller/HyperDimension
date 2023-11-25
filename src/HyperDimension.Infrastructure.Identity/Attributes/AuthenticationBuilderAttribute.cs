namespace HyperDimension.Infrastructure.Identity.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuthenticationBuilderAttribute(string name, Type optionsType)
    : Attribute
{
    public string Name => name;
    public Type OptionsType => optionsType;
}
