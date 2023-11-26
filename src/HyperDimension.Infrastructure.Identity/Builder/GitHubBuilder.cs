using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("GitHub", typeof(GitHubProviderProviderOptions))]
public class GitHubBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        var opt = (GitHubProviderProviderOptions)options;

        builder.AddGitHub(id, name, o =>
        {
            o.SaveTokens = true;

            o.ClientId = opt.ClientId;
            o.ClientSecret = opt.ClientSecret;

            o.CallbackPath = $"/identity/sso/callback/{id}";
        });
    }
}
