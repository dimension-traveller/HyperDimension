using HyperDimension.Application.Core.Identity.User.Login;
using HyperDimension.Application.Core.Identity.User.Logout;
using HyperDimension.Application.Core.Identity.User.PasswordReset;
using HyperDimension.Application.Core.Identity.User.PasswordResetRequest;
using HyperDimension.Application.Core.Identity.User.Registration;
using HyperDimension.Application.Core.Identity.User.Verification;
using HyperDimension.Presentation.Common.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/user")]
public class UserController : HyperDimensionControllerBase
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("registration")]
    public Task<IActionResult> RegistrationAsync(UserRegistration request)
    {
        return SendAsync(request);
    }

    [HttpPost("login")]
    public Task<IActionResult> LoginAsync(UserLogin request)
    {
        return SendAsync(request);
    }

    [HttpPost("logout")]
    public Task<IActionResult> LoginAsync(UserLogout request)
    {
        return SendAsync(request);
    }

    [HttpPost("verification")]
    public Task<IActionResult> VerificationAsync(UserVerification request)
    {
        return SendAsync(request);
    }

    [HttpPost("password/request")]
    public Task<IActionResult> PasswordResetRequestAsync(PasswordResetRequest request)
    {
        return SendAsync(request);
    }

    [HttpPost("password/reset")]
    public Task<IActionResult> PasswordResetAsync(PasswordReset request)
    {
        return SendAsync(request);
    }
}
