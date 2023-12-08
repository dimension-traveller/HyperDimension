﻿using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public string ConcurrencyStamp { get; set; } = string.Empty;

    public string SecurityStamp { get; set; } = string.Empty;

    public bool TwoFactorEnabled { get; set; }

    public List<Role> Roles { get; set; } = [];

    public List<WebAuthn> WebAuthnDevices { get; set; } = [];
}
