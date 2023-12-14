using System.Security.Claims;
using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("GitHub")]
public class GitHubBuilder : IAuthenticationProviderBuilder<GitHubProviderOptions>
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, IdentityProviderOptions metadata, GitHubProviderOptions options)
    {
        builder.Services.AddKeyedSingleton(metadata.Id, new ExternalClaimsOptions
        {
            UniqueId =  ClaimTypes.NameIdentifier,
            Username = ClaimTypes.Name,
            DisplayName = "urn:github:name",
            Email =  ClaimTypes.Email
        });

        builder.AddGitHub(metadata.Id, metadata.Name, o =>
        {
            o.ClientId = options.ClientId;
            o.ClientSecret = options.ClientSecret;

            o.CallbackPath = $"/identity/sso/callback/{metadata.Id}";
        });
    }
}
