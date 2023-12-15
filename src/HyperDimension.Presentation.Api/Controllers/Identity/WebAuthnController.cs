using HyperDimension.Application.Core.Identity.WebAuthn.Authentication;
using HyperDimension.Application.Core.Identity.WebAuthn.AuthenticationAssertion;
using HyperDimension.Application.Core.Identity.WebAuthn.Registration;
using HyperDimension.Application.Core.Identity.WebAuthn.RegistrationAttestation;
using HyperDimension.Presentation.Common.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/webauthn")]
public class WebAuthnController : HyperDimensionControllerBase
{
    public WebAuthnController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("registration")]
    public Task<IActionResult> RegistrationAsync(WebAuthnRegistration request)
    {
        return SendAsync(request);
    }

    [HttpPost("registration/attestation")]
    public Task<IActionResult> RegistrationAttestationAsync(WebAuthnRegistrationAttestation request)
    {
        return SendAsync(request);
    }

    [HttpPost("authentication")]
    public Task<IActionResult> LoginInitializeAsync(WebAuthnAuthentication request)
    {
        return SendAsync(request);
    }

    [HttpPost("authentication/assertion")]
    public Task<IActionResult> LoginVerifyAsync(WebAuthnAuthenticationAssertion request)
    {
        return SendAsync(request);
    }
}
