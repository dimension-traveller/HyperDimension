using HyperDimension.Common.Constants;

namespace HyperDimension.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireAuthenticationAttribute : Attribute
{
    public string[] Schemas { get; set; }

    public RequireAuthenticationAttribute(params string[] schemas)
    {
        Schemas = schemas.Length == 0
            ? [IdentityConstants.IdentitySchema]
            : schemas;
    }
}
