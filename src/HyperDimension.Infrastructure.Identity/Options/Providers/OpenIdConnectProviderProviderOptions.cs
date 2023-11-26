using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HyperDimension.Infrastructure.Identity.Options.Providers;

public class OpenIdConnectProviderProviderOptions : OAuthProviderOptions
{
    public string Authority { get; set; } = string.Empty;

    public string[] Scope { get; set; } = [];

    public OpenIdConnectConfiguration? OpenIdConnectConfiguration { get; set; }
}
