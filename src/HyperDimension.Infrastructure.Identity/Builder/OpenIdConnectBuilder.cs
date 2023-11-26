using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("OpenIdConnect", typeof(OpenIdConnectProviderProviderOptions))]
public class OpenIdConnectBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        var opt = (OpenIdConnectProviderProviderOptions)options;

        builder.AddOpenIdConnect(id, name, o =>
        {
            o.SaveTokens = true;

            o.ClientId = opt.ClientId;
            o.ClientSecret = opt.ClientSecret;

            o.Scope.Clear();
            foreach (var s in opt.Scope)
            {
                o.Scope.Add(s);
            }

            o.ClientId = opt.ClientId;
            o.ClientSecret = opt.ClientSecret;

            o.CallbackPath = $"/identity/sso/callback/{id}";

            o.Authority = opt.Authority;

            if (opt.OpenIdConnectConfiguration is not null)
            {
                if (
                    string.IsNullOrEmpty(opt.OpenIdConnectConfiguration.AuthorizationEndpoint) ||
                    string.IsNullOrEmpty(opt.OpenIdConnectConfiguration.TokenEndpoint) ||
                    string.IsNullOrEmpty(opt.OpenIdConnectConfiguration.UserInfoEndpoint))
                {
                    return;
                }

                o.Configuration = opt.OpenIdConnectConfiguration;
            }
        });
    }
}
