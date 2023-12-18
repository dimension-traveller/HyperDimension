using HyperDimension.Infrastructure.Identity.Authenticator.StaticToken;
using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Authenticator;

public static class AuthenticatorExtensions
{
    public static AuthenticationBuilder AddStaticToken(
        this AuthenticationBuilder builder,
        string authenticationSchema = "Static Token",
        string? displayName = null,
        Action<StaticTokenOptions>? configureOptions = null)
    {
        builder.AddScheme<StaticTokenOptions, StaticTokenHandler>(authenticationSchema, displayName, configureOptions);

        return builder;
    }
}
