using HyperDimension.Infrastructure.Identity.Schemas.ProxyServer;
using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Schemas;

public static class AuthenticationSchemaExtensions
{
    public static AuthenticationBuilder AddProxyServer(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        string? displayName,
        Action<ProxyServerSchemaOptions> configureOptions)
    {
        builder.AddScheme<ProxyServerSchemaOptions, ProxyServerHandler>
            (authenticationScheme, displayName, configureOptions);

        return builder;
    }
}
