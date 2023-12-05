using HyperDimension.Common.Attributes;

namespace HyperDimension.Infrastructure.Database.Options;

[OptionSection("DataProtection")]
public class DatabaseDataProtectionOptions
{
    public bool EnableCertificate { get; set; }

    public DataProtectionCertificate Certificate { get; set; } = new();

    public DataProtectionCertificate[] RotatedCertificates { get; set; } = [];
}
