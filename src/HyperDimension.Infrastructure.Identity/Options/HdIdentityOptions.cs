using HyperDimension.Common.Attributes;

namespace HyperDimension.Infrastructure.Identity.Options;

[OptionSection("Identity")]
public class HdIdentityOptions
{
    public TokenOptions Token { get; set; } = new();

    public WebAuthnOptions WebAuthn { get; set; } = new();

    public ICollection<IdentityProviderOptions> Providers { get; set; } = [];
}
