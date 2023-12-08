using HyperDimension.Common.Attributes;

namespace HyperDimension.Common.Options;

[OptionSection("Application")]
public class ApplicationOptions
{
    public string FrontendUrl { get; set; } = "http://localhost:3000";
}
