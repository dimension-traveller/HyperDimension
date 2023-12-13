using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Domain.Email;

public class PasswordResetCode : IEmailTemplate
{
    public User User { get; set; } = null!;

    public string Code { get; set; } = null!;
}
