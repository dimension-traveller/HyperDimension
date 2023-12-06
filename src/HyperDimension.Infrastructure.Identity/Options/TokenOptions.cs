namespace HyperDimension.Infrastructure.Identity.Options;

public class TokenOptions
{
    public int AccessTokenExpiration { get; set; } = 60;

    public int RefreshTokenExpiration { get; set; } = 60 * 24 * 7;
}
