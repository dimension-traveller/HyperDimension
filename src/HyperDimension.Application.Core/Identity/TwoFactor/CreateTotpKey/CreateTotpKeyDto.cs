using System.Text.Json.Serialization;

namespace HyperDimension.Application.Core.Identity.TwoFactor.CreateTotpKey;

public class CreateTotpKeyDto
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; } = null!;
}
