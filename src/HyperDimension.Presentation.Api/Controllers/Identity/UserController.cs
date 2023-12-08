using HyperDimension.Application.Core.Identity.UserManagement.Registration;
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
}
