using System.Text;
using System.Text.Json.Serialization;
using Fido2NetLib;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.WebAuthn.RegistrationAttestation;

public class WebAuthnRegistrationAttestation : IRequest<IActionResult>
{
    [FromQuery(Name = "challenge")]
    public string Challenge { get; set; } = string.Empty;

    [FromBody]
    public WebAuthnRegistrationAttestationBody Body { get; set; } = new();
}

public class WebAuthnRegistrationAttestationBody
{
    [JsonPropertyName("attestation_response")]
    public AuthenticatorAttestationRawResponse AttestationResponse { get; set; } = new();
}

public class WebAuthnRegistrationAttestationHandler : IRequestHandler<WebAuthnRegistrationAttestation, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IStringLocalizer<WebAuthnRegistrationAttestationHandler> _localizer;
    private readonly IWebAuthnAuthenticationService _webAuthnAuthenticationService;

    public WebAuthnRegistrationAttestationHandler(
        IHyperDimensionDbContext dbContext,
        IStringLocalizer<WebAuthnRegistrationAttestationHandler> localizer,
        IWebAuthnAuthenticationService webAuthnAuthenticationService)
    {
        _dbContext = dbContext;
        _localizer = localizer;
        _webAuthnAuthenticationService = webAuthnAuthenticationService;
    }

    public async Task<IActionResult> Handle(WebAuthnRegistrationAttestation request, CancellationToken cancellationToken)
    {
        var result = await _webAuthnAuthenticationService
            .VerifyRegistrationAssertionAsync(request.Challenge, request.Body.AttestationResponse);

        if (result.IsFailure)
        {
            return new ErrorMessageResult(_localizer["Failed to pass WebAuthn attestation."]).ToBadRequest();
        }

        var attestationVerificationResult = result.Unwrap();

        var device = new Domain.Entities.Identity.WebAuthn
        {
            CredentialId = attestationVerificationResult.CredentialId,
            PublicKey = attestationVerificationResult.PublicKey,
            UserHandle = attestationVerificationResult.User.Id,
            SignatureCounter = attestationVerificationResult.Counter,
            CredType = attestationVerificationResult.CredType,
            RegDate = DateTimeOffset.UtcNow,
            AaGuid = attestationVerificationResult.Aaguid
        };

        var email = Encoding.UTF8.GetString(attestationVerificationResult.User.Id);

        var user = await _dbContext.Users
            .Include(x => x.WebAuthnDevices)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Username = attestationVerificationResult.User.Name,
                Email = email,
                DisplayName = attestationVerificationResult.User.DisplayName,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                WebAuthnDevices = [device]
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            user.WebAuthnDevices.Add(device);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SignInResult(user.CreateIdentityClaimsPrincipal());
    }
}
