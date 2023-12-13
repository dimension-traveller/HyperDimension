using HyperDimension.Common.Attributes;

namespace HyperDimension.Common.Options;

[OptionSection("Metadata")]
public class MetadataOptions
{
    public string SiteName { get; set; } = "Hyper Dimension";

    public string Issuer { get; set; } = "Dimension Traveller";
}
