namespace HyperDimension.Infrastructure.Identity.Options;

public class ExternalClaimsOptions
{
    public string UniqueId { get; set; } = "sub";

    public string Username { get; set; } = "preferred_username";

    public string DisplayName { get; set; } = "name";

    public string Email { get; set; } = "email";
}
