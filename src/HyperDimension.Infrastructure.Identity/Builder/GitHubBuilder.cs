using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("GitHub", typeof(GitHubOptions))]
public class GitHubBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        var opt = (GitHubOptions)options;

        var cookieSchemaId = $"{id}-cookie";

        builder
            .AddCookie(cookieSchemaId)
            .AddGitHub(id, name, o =>
            {
                o.SignInScheme = cookieSchemaId;
                o.SaveTokens = true;

                o.ClientId = opt.ClientId;
                o.ClientSecret = opt.ClientSecret;

                o.CallbackPath = $"/identity/signin/callback/{id}";
            });
    }
}
