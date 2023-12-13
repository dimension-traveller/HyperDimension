namespace HyperDimension.Common.Constants;

public static class SecurityTokenConstants
{
    public const int TokenLifetimeWindow = 60;

    public const int AccountVerificationTokenLength = 16;

    public const int AccountVerificationTokenLifetime = 30 * 60;

    public const int PasswordResetTokenLength = 16;

    public const int PasswordResetTokenLifetime = 5 * 60;

    public const int TwoFactorAuthenticationTokenLength = 8;

    public const int TwoFactorAuthenticationTokenLifetime = 5 * 60;
}
