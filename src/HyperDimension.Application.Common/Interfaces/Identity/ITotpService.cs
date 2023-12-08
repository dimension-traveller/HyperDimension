namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface ITotpService
{
    public byte[] GenerateKey();

    public IReadOnlyList<string> GenerateRecoveryCodes();

    public bool VerifyTotpCode(byte[] key, string code, DateTimeOffset time);
}
