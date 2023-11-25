using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using HyperDimension.Infrastructure.Identity.Schemas;
using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("ProxyServer", typeof(ProxyServerOptions))]
public class ProxyServerBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema => true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        var opt = (ProxyServerOptions)options;

        builder.AddProxyServer(id, name, o =>
        {
            o.UsernameHeader = opt.UsernameHeader;
            o.EmailHeader = opt.EmailHeader;
        });
    }
}
