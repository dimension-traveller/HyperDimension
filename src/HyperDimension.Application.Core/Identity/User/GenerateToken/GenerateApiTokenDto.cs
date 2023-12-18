using System.Text.Json.Serialization;

namespace HyperDimension.Application.Core.Identity.User.GenerateToken;

public class GenerateApiTokenDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("expire_at")]
    public DateTimeOffset ExpireAt { get; set; }
}
