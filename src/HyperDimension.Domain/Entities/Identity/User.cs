using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public List<Role> Roles { get; set; } = [];

    public List<WebAuthn> WebAuthnDevices { get; set; } = [];
}
