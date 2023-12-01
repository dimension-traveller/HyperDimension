using System.Security.Cryptography;

namespace HyperDimension.Common.Utilities;

public static class RandomUtility
{
    private static ReadOnlySpan<char> TokenCharacters =>
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".AsSpan();

    public static string GenerateToken(int length)
    {
        return RandomNumberGenerator.GetString(TokenCharacters, length);
    }
}
