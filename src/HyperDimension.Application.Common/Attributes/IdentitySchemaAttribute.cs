namespace HyperDimension.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class IdentitySchemaAttribute : Attribute
{
    public string Schema { get; set; }

    public IdentitySchemaAttribute(string schema)
    {
        Schema = schema;
    }
}
