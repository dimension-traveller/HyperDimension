using HyperDimension.Application.Core.Identity.Sso.Login;
using HyperDimension.Application.Core.Identity.Sso.LoginCallback;
using HyperDimension.Application.Core.Identity.Sso.LoginConfirm;
using HyperDimension.Presentation.Common.Abstract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/sso")]
public class SsoController : HyperDimensionControllerBase
{
    public SsoController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("login")]
    public Task<IActionResult> LoginAsync(SsoLogin request)
    {
        return SendAsync(request);
    }

    [HttpGet("callback")]
    public Task<IActionResult> LoginCallbackAsync(SsoLoginCallback request)
    {
        return SendAsync(request);
    }

    [HttpGet("confirm")]
    public Task<IActionResult> LoginConfirmAsync(SsoLoginConfirm request)
    {
        return SendAsync(request);
    }
}
