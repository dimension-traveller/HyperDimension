using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Schemas.ProxyServer;

public class ProxyServerSchemaOptions : AuthenticationSchemeOptions
{
    public string UsernameHeader { get; set; } = "X-Remote-User";

    public string EmailHeader { get; set; } = "X-Remote-Email";
}
