using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Exceptions;
using HyperDimension.Infrastructure.Identity.Options;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("OpenIdConnect")]
public class OpenIdConnectBuilder : IAuthenticationProviderBuilder<OpenIdConnectProviderOptions>
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, IdentityProviderOptions metadata, OpenIdConnectProviderOptions options)
    {
        builder.AddOpenIdConnect(metadata.Id, metadata.Name, o =>
        {
            o.SaveTokens = true;

            o.ClientId = options.ClientId;
            o.ClientSecret = options.ClientSecret;

            o.Scope.Clear();
            foreach (var s in options.Scope)
            {
                o.Scope.Add(s);
            }

            o.MapInboundClaims = false;

            o.CallbackPath = $"/identity/sso/callback/{metadata.Id}";

            o.Authority = options.Authority;

            if (string.IsNullOrEmpty(options.MetadataAddress) is false)
            {
                o.MetadataAddress = options.MetadataAddress;
            }
            else
            {
                if (options.OpenIdConnectConfiguration is null)
                {
                    throw new AuthenticationNotSupportedException(metadata.Type,
                        "MetadataAddress or OpenIdConnectConfiguration must be provided.");
                }

                o.Configuration = options.OpenIdConnectConfiguration;
            }
        });
    }
}
