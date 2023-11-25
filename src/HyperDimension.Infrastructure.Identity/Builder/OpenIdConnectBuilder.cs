using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("OpenId", typeof(OpenIdConnectOptions))]
public class OpenIdConnectBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        var opt = (OpenIdConnectOptions)options;

        builder.AddOpenIdConnect(id, name, o =>
        {
            o.ClientId = opt.ClientId;
            o.ClientSecret = opt.ClientSecret;


            o.CallbackPath = $"/identity/signin/callback/{id}";
        });
    }
}
