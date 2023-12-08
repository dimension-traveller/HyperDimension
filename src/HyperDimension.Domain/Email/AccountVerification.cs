using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Attributes;

namespace HyperDimension.Domain.Email;

[EmailTemplate("HyperDimension.Domain.Templates.Email.AccountVerification.cshtml")]
public class AccountVerification : IEmailTemplate
{
    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string ActivationUrl { get; set; } = string.Empty;

    public string SiteName { get; set; } = string.Empty;
}
