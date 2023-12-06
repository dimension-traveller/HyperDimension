using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("Discord")]
public class DiscordBuilder : IAuthenticationProviderBuilder<DiscordProviderOptions>
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, IdentityProviderOptions metadata, DiscordProviderOptions options)
    {
        builder.AddDiscord(metadata.Id, metadata.Name, o =>
        {
            o.ClientId = options.ClientId;
            o.ClientSecret = options.ClientSecret;

            o.CallbackPath = $"/identity/sso/callback/{metadata.Id}";
        });
    }
}
