using HyperDimension.Application.Core.Identity.TwoFactor.CreateTotpKey;
using HyperDimension.Application.Core.Identity.TwoFactor.DisableTwoFactor;
using HyperDimension.Application.Core.Identity.TwoFactor.EnableTwoFactor;
using HyperDimension.Application.Core.Identity.TwoFactor.GetStatus;
using HyperDimension.Application.Core.Identity.TwoFactor.Login;
using HyperDimension.Application.Core.Identity.TwoFactor.SendEmailCode;
using HyperDimension.Presentation.Common.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/2fa")]
public class TwoFactorController : HyperDimensionControllerBase
{
    public TwoFactorController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    public Task<IActionResult> LoginAsync(TwoFactorLogin request)
    {
        return SendAsync(request);
    }

    [HttpPost("email")]
    public Task<IActionResult> SendEmailCodeAsync(SendEmailCode request)
    {
        return SendAsync(request);
    }

    [HttpPost("totp")]
    public Task<IActionResult> CreateTotpKeyAsync(CreateTotpKey request)
    {
        return SendAsync(request);
    }

    [HttpGet("status")]
    public Task<IActionResult> GetStatusAsync(GetTwoFactorStatus request)
    {
        return SendAsync(request);
    }

    [HttpPost("enable")]
    public Task<IActionResult> EnableAsync(EnableTwoFactor request)
    {
        return SendAsync(request);
    }

    [HttpPost("disable")]
    public Task<IActionResult> DisableAsync(DisableTwoFactor request)
    {
        return SendAsync(request);
    }
}
