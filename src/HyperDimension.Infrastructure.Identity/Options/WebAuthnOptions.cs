using HyperDimension.Common.Attributes;

namespace HyperDimension.Infrastructure.Identity.Options;

[OptionSection("Identity:WebAuthn")]
public class WebAuthnOptions
{
    public uint Timeout { get; set; } = 60000;

    public string ServerIcon { get; set; } = string.Empty;

    public string ServerDomain { get; set; } = "localhost";

    public string ServerName { get; set; } = "Hyper Dimension";

    public List<string> Origins { get; set; } = [];
}
