using System.Text.Json.Serialization;

namespace HyperDimension.Application.Core.Identity.User.Registration;

public class UserRegistrationDto
{
    [JsonPropertyName("email_verification_required")]
    public bool EmailVerificationRequired { get; set; }
}
