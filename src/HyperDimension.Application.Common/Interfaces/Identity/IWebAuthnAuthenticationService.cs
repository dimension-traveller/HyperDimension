using Fido2NetLib;
using Fido2NetLib.Objects;
using HyperDimension.Common;

namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface IWebAuthnAuthenticationService
{
    public Task<CredentialCreateOptions> CreateRegistrationOptionsAsync(
        Fido2User user,
        IEnumerable<byte[]> existingCredentialIds);

    public Task<Result<AttestationVerificationSuccess>> VerifyRegistrationAssertionAsync(
        string challenge,
        AuthenticatorAttestationRawResponse attestationResponse);

    public Task<AssertionOptions> CreateAuthenticationAssertionOptionsAsync(IEnumerable<byte[]> existingCredentialIds);

    public Task<Result<Guid>> VerifyAuthenticationAssertionAsync(
        string challenge,
        AuthenticatorAssertionRawResponse assertionResponse);
}
