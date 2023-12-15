using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.WebAuthn.Authentication;

public class WebAuthnAuthentication : IRequest<IActionResult>
{
    [FromBody]
    public WebAuthnAuthenticationBody Body { get; set; } = new();
}

public class WebAuthnAuthenticationBody
{
    [JsonPropertyName("login_name")]
    public string LoginName { get; set; } = string.Empty;
}

public class WebAuthnAuthenticationHandler : IRequestHandler<WebAuthnAuthentication, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IStringLocalizer<WebAuthnAuthenticationHandler> _localizer;
    private readonly IWebAuthnAuthenticationService _webAuthnAuthenticationService;

    public WebAuthnAuthenticationHandler(
        IHyperDimensionDbContext dbContext,
        IStringLocalizer<WebAuthnAuthenticationHandler> localizer,
        IWebAuthnAuthenticationService webAuthnAuthenticationService)
    {
        _dbContext = dbContext;
        _localizer = localizer;
        _webAuthnAuthenticationService = webAuthnAuthenticationService;
    }

    public async Task<IActionResult> Handle(WebAuthnAuthentication request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.WebAuthnDevices)
            .FirstOrDefaultAsync(x =>
                x.Username == request.Body.LoginName ||
                x.Email == request.Body.LoginName, cancellationToken);

        if (user is null)
        {
            return new ErrorMessageResult(_localizer["User not found."]).ToBadRequest();
        }

        if (user.WebAuthnDevices.Count == 0)
        {
            return new ErrorMessageResult(_localizer["User has no WebAuthn device registered."]).ToBadRequest();
        }

        var credentialIds = user.WebAuthnDevices
            .Select(x => x.CredentialId);

        var assertionOptions = await _webAuthnAuthenticationService.CreateAuthenticationAssertionOptionsAsync(credentialIds);

        return new OkObjectResult(assertionOptions);
    }
}

