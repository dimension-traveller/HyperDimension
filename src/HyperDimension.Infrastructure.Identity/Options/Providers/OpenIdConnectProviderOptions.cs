using HyperDimension.Infrastructure.Identity.Abstract;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HyperDimension.Infrastructure.Identity.Options.Providers;

public class OpenIdConnectProviderOptions : OAuthProviderOptions, IIdentityProviderConfig
{
    public string Authority { get; set; } = string.Empty;

    public string MetadataAddress { get; set; } = string.Empty;

    public string[] Scope { get; set; } = [];

    public OpenIdConnectConfiguration? OpenIdConnectConfiguration { get; set; }
}
