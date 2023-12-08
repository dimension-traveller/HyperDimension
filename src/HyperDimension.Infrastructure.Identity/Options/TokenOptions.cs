namespace HyperDimension.Infrastructure.Identity.Options;

public class TokenOptions
{
    public int AccessTokenExpiration { get; set; } = 3600;

    public int RefreshTokenExpiration { get; set; } = 2592000;
}
