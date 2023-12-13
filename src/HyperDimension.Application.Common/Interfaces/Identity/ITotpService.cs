namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface ITotpService
{
    public IEnumerable<byte> GenerateKey();

    public string GetTotpUri(byte[] key, string email);

    public IEnumerable<string> GenerateRecoveryCodes();

    public bool VerifyTotpCode(byte[] key, string code, DateTimeOffset time);
}
