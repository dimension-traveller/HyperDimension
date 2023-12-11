using System.Diagnostics.CodeAnalysis;
using System.Text;
using HyperDimension.Application.Common.Interfaces.Identity;
using Isopoh.Cryptography.Argon2;

namespace HyperDimension.Infrastructure.Identity.Services;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public class PasswordHashService : IPasswordHashService
{
    private static Argon2Config CreateConfig() => new()
    {
        Type = Argon2Type.DataIndependentAddressing,
        Version = Argon2Version.Nineteen,
        TimeCost = 10,
        MemoryCost = 32768,
        Lanes = 5,
        Threads = Environment.ProcessorCount,
        HashLength = 20
    };

    public string HashPassword(string password, string salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Encoding.UTF8.GetBytes(salt);

        var config = CreateConfig();

        config.Password = passwordBytes;
        config.Salt = saltBytes;

        using var argon2 = new Argon2(config);
        using var hashArray = argon2.Hash();

        var hashString = config.EncodeString(hashArray.Buffer);

        return hashString;
    }

    public bool VerifyPassword(string password, string salt, string passwordHash)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = Encoding.UTF8.GetBytes(salt);

        var config = CreateConfig();

        config.Password = passwordBytes;
        config.Salt = saltBytes;

        try
        {
            if (config.DecodeString(passwordHash, out var hashArray) && hashArray is not null)
            {
                var argon2 = new Argon2(config);
                using var hashToVerify = argon2.Hash();

                if (Argon2.FixedTimeEquals(hashArray, hashToVerify))
                {
                    return true;
                }
            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }
}
