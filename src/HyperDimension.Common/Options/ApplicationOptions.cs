using HyperDimension.Common.Attributes;

namespace HyperDimension.Common.Options;

[OptionSection("Application")]
public class ApplicationOptions
{
    public string FrontendUrl { get; set; } = "http://localhost:3000";

    public string AccountVerificationUrl { get; set; } = "http://localhost:3000/account/verify?token={TOKEN}";
}
