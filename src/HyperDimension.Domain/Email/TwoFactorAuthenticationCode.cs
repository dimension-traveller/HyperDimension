using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Attributes;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Domain.Email;

[EmailTemplate("HyperDimension.Domain.Templates.Email.TwoFactorAuthenticationCode.cshtml", "Two Factor Authentication Code Email")]
public class TwoFactorAuthenticationCode : IEmailTemplate
{
    public User User { get; set; } = null!;

    public string Code { get; set; } = null!;
}
