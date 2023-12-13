using HyperDimension.Application.Core.Identity.UserManagement.Login;
using HyperDimension.Application.Core.Identity.UserManagement.PasswordReset;
using HyperDimension.Application.Core.Identity.UserManagement.PasswordResetRequest;
using HyperDimension.Application.Core.Identity.UserManagement.Registration;
using HyperDimension.Application.Core.Identity.UserManagement.Verification;
using HyperDimension.Presentation.Common.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/user")]
public class UserManagementController : HyperDimensionControllerBase
{
    public UserManagementController(IMediator mediator) : base(mediator)
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
