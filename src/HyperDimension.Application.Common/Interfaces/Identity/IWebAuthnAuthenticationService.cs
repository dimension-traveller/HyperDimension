using Fido2NetLib;
using HyperDimension.Common;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface IWebAuthnAuthenticationService
{
    public Task<CredentialCreateOptions> CreateWebAuthnRegistrationOptionsAsync(
        Fido2User user,
        IEnumerable<byte[]> existingCredentialIds);

    public Task<Result<User>> VerifyWebAuthnRegistrationAsync(
        string cacheKey,
        AuthenticatorAttestationRawResponse attestationResponse);

    public Task<AssertionOptions> CreateWebAuthnAssertionOptionsAsync(IEnumerable<byte[]> existingCredentialIds);

    public Task<Result<Guid>> VerifyWebAuthnAssertionAsync(
        string cacheKey,
        AuthenticatorAssertionRawResponse assertionResponse);
}
