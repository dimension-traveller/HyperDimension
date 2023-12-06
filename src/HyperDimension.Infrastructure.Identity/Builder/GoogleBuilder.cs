using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Options;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

public class GoogleBuilder : IAuthenticationProviderBuilder<GoogleProviderOptions>
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, IdentityProviderOptions metadata, GoogleProviderOptions options)
    {
        builder.AddGoogle(metadata.Id, metadata.Name, o =>
        {
            o.ClientId = options.ClientId;
            o.ClientSecret = options.ClientSecret;

            o.CallbackPath = $"/identity/sso/callback/{metadata.Id}";
        });
    }
}
