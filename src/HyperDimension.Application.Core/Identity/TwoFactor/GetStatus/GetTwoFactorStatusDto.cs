using System.Text.Json.Serialization;

namespace HyperDimension.Application.Core.Identity.TwoFactor.GetStatus;

public class GetTwoFactorStatusDto
{
    [JsonPropertyName("totp_enabled")]
    public bool TotpEnabled { get; set; }

    [JsonPropertyName("email_enabled")]
    public bool EmailEnabled { get; set; }
}
