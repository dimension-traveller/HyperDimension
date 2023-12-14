using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Application.Core.Identity.Sso.Login;

public class SsoLogin : IRequest<IActionResult>
{
    [FromQuery(Name = "schema")]
    public string Schema { get; set; } = string.Empty;

    [FromQuery(Name = "return_url")]
    public string? ReturnUrl { get; set; }
}

public class SsoLoginHandler : IRequestHandler<SsoLogin, IActionResult>
{
    private readonly IUrlHelper _urlHelper;

    public SsoLoginHandler(IUrlHelper urlHelper)
    {
        _urlHelper = urlHelper;
    }

    public Task<IActionResult> Handle(SsoLogin request, CancellationToken cancellationToken)
    {
        var redirectUrl = _urlHelper.Action("LoginCallback", "Sso", new
        {
            return_url = request.ReturnUrl
        });

        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl,
            Items =
            {
                ["SSO:Schema"] = request.Schema
            }
        };

        return Task.FromResult<IActionResult>(new ChallengeResult(request.Schema, properties));
    }
}
