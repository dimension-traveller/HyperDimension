using System.Security.Cryptography;

namespace HyperDimension.Common.Utilities;

public static class RandomUtility
{
    private static ReadOnlySpan<char> TokenCharacters =>
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".AsSpan();

    private static ReadOnlySpan<char> NumericTokenCharacters =>
        "0123456789".AsSpan();

    public static string GenerateToken(int length, Func<string, bool>? collisionCheck = null)
    {
        return Generate(length, TokenCharacters, collisionCheck);
    }

    public static string GenerateNumericToken(int length, Func<string, bool>? collisionCheck = null)
    {
        return Generate(length, NumericTokenCharacters, collisionCheck);
    }

    public static string Generate(int length, ReadOnlySpan<char> allowedCharacters, Func<string, bool>? collisionCheck = null)
    {
        var check = collisionCheck ?? (_ => false);
        var token = RandomNumberGenerator.GetString(allowedCharacters, length);

        while (check.Invoke(token))
        {
            token = RandomNumberGenerator.GetString(allowedCharacters, length);
        }

        return token;
    }
}
