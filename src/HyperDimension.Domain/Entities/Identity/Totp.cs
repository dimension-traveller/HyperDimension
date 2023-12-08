using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class Totp : BaseEntity
{
    public byte[] Key { get; set; } = [];

    public DateTimeOffset RegistrationTime { get; set; }

    public List<TotpRecoveryCode> RecoveryCodes { get; set; } = [];
}
