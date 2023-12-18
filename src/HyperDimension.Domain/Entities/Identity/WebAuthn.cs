using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Domain.Entities.Identity;

public class WebAuthn : BaseEntity
{
    public byte[] CredentialId { get; set; } = [];

    public byte[] PublicKey { get; set; } = [];

    public byte[] UserHandle { get; set; } = [];

    public uint SignatureCounter { get; set; }

    public string CredType { get; set; } = string.Empty;

    public DateTimeOffset RegDate { get; set; }

    public Guid AaGuid { get; set; }

    public User User { get; set; } = null!;
}
