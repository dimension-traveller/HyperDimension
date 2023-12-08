namespace HyperDimension.Application.Common.Models.Email;

public class AccountVerification
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string VerificationUrl { get; set; } = string.Empty;
}
