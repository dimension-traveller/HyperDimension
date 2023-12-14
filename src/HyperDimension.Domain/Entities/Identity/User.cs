using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public string SecurityStamp { get; set; } = string.Empty;

    public bool TwoFactorEmailEnabled { get; set; }

    public bool TwoFactorTotpEnabled { get; set; }

    public int FailedAccessAttempts { get; set; }

    public DateTimeOffset? LockoutEndAt { get; set; }

    public Totp Totp { get; set; } = new();

    public List<Role> Roles { get; set; } = [];

    public List<ApiToken> ApiTokens { get; set; } = [];

    public List<ExternalProvider> ExternalProviders { get; set; } = [];

    public List<WebAuthn> WebAuthnDevices { get; set; } = [];
}
