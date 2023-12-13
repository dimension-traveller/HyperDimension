using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Attributes;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Domain.Email;

[EmailTemplate("HyperDimension.Domain.Templates.Email.AccountVerification.cshtml", "Account Verification Email")]
public class AccountVerification : IEmailTemplate
{
    public User User { get; set; } = null!;

    public string Token { get; set; } = string.Empty;
}
