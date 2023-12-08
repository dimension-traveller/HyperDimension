using System.Text.Json.Serialization;

namespace HyperDimension.Application.Core.Identity.UserManagement.Registration;

public class UserRegistrationDto
{
    [JsonPropertyName("email_verification_required")]
    public bool EmailVerificationRequired { get; set; }
}
