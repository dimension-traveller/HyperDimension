using HyperDimension.Application.Common.Interfaces.Identity;
using Isopoh.Cryptography.Argon2;

namespace HyperDimension.Infrastructure.Identity.Services;

public class PasswordHashService : IPasswordHashService
{
    public string HashPassword(string password, string salt)
    {
        var saltedPassword = $"{password}{salt}";
        return Argon2.Hash(saltedPassword);
    }

    public bool VerifyPassword(string password, string salt, string passwordHash)
    {
        var saltedPassword = $"{password}{salt}";
        return Argon2.Verify(saltedPassword, passwordHash);
    }
}
