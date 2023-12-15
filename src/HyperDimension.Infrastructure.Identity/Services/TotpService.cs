using System.Text.Encodings.Web;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common.Options;
using HyperDimension.Common.Utilities;
using OtpNet;

namespace HyperDimension.Infrastructure.Identity.Services;

public class TotpService : ITotpService
{
    private readonly MetadataOptions _metadataOptions;

    public TotpService(MetadataOptions metadataOptions)
    {
        _metadataOptions = metadataOptions;
    }

    public IEnumerable<byte> GenerateKey()
    {
        return KeyGeneration.GenerateRandomKey(20);
    }

    public string GetTotpUri(byte[] key, string email)
    {
        var stringKey = Base32Encoding.ToString(key);
        var label = UrlEncoder.Default.Encode(email);
        var issuer = UrlEncoder.Default.Encode(_metadataOptions.Issuer);

        return $"otpauth://totp/{label}?secret={stringKey}&issuer={issuer}";
    }

    public IEnumerable<string> GenerateRecoveryCodes()
    {
        const int recoveryCodeCount = 8;
        var recoveryCodes = new List<string>();

        for (var i = 1; i <= recoveryCodeCount; i++)
        {
            var code = RandomUtility.GenerateNumericToken(7, s => recoveryCodes.Contains(s));
            recoveryCodes.Add(code);
        }

        return recoveryCodes;
    }

    public bool VerifyTotpCode(byte[] key, string code, DateTimeOffset time)
    {
        var totp = new Totp(key);
        var calculatedCode = totp.ComputeTotp(time.DateTime);

        return code == calculatedCode;
    }
}
