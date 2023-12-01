using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/webauthn")]
public class WebAuthnController : ControllerBase
{
    [HttpPost("register/initialize")]
    public Task<IActionResult> RegisterInitializeAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPost("register/verify")]
    public Task<IActionResult> RegisterVerifyAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPost("login/initialize")]
    public Task<IActionResult> LoginInitializeAsync()
    {
        throw new NotImplementedException();
    }

    [HttpPost("login/verify")]
    public Task<IActionResult> LoginVerifyAsync()
    {
        throw new NotImplementedException();
    }
}
