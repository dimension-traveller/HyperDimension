using Fido2NetLib;
using HyperDimension.Common;

namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionWebAuthnAuthenticationService
{
    public Task<CredentialCreateOptions> CreateWebAuthnRegistrationOptionsAsync(Fido2User user);
    public Task<Result<Guid>> VerifyWebAuthnRegistrationAsync(string cacheKey, AuthenticatorAttestationRawResponse attestationResponse);
    public Task<Result<AssertionOptions>> CreateWebAuthnAssertionOptionsAsync(string userIdentifier);
    public Task<Result<Guid>> VerifyWebAuthnAssertionAsync(string cacheKey, AuthenticatorAssertionRawResponse assertionResponse);
}
