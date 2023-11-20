namespace HyperDimension.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class OptionSectionAttribute(string name) : Attribute
{
    public string SectionName => name;
}
