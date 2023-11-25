namespace HyperDimension.Infrastructure.Identity.Options.Providers;

public class ProxyServerOptions
{
    public string UsernameHeader { get; set; } = "X-Remote-Username";

    public string EmailHeader { get; set; } = "X-Remote-Email";
}
