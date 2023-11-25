namespace HyperDimension.Infrastructure.Identity.Options.Providers;

public class LocalOptions
{
    public string Issuer { get; set; } = "HyperDimension.Identity";

    public string Audience { get; set; } = "HyperDimension";

    public string Secret { get; set; } = "YOUR_SUPER_SECRET_JWT_SIGNING_SECRET_KEY";
}
