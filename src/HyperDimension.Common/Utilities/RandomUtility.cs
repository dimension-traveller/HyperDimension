using System.Security.Cryptography;

namespace HyperDimension.Common.Utilities;

public static class RandomUtility
{
    private static ReadOnlySpan<char> TokenCharacters =>
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-".AsSpan();

    private static ReadOnlySpan<char> NumericTokenCharacters =>
        "0123456789".AsSpan();

    public static string GenerateToken(int length)
    {
        return RandomNumberGenerator.GetString(TokenCharacters, length);
    }

    public static string GenerateNumericToken(int length)
    {
        return RandomNumberGenerator.GetString(NumericTokenCharacters, length);
    }
}
