using System.Text.Json.Serialization;
using Fido2NetLib;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.WebAuthn.AuthenticationAssertion;

public class WebAuthnAuthenticationAssertion : IRequest<IActionResult>
{
    [FromQuery(Name = "challenge")]
    public string Challenge { get; set; } = string.Empty;

    [FromBody]
    public WebAuthnAuthenticationAssertionBody Body { get; set; } = new();
}

public class WebAuthnAuthenticationAssertionBody
{
    [JsonPropertyName("assertion_response")]
    public AuthenticatorAssertionRawResponse AssertionResponse { get; set; } = new();
}

public class WebAuthnAuthenticationAssertionHandler : IRequestHandler<WebAuthnAuthenticationAssertion, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IStringLocalizer<WebAuthnAuthenticationAssertionHandler> _localizer;
    private readonly IWebAuthnAuthenticationService _webAuthnAuthenticationService;

    public WebAuthnAuthenticationAssertionHandler(
        IHyperDimensionDbContext dbContext,
        IStringLocalizer<WebAuthnAuthenticationAssertionHandler> localizer,
        IWebAuthnAuthenticationService webAuthnAuthenticationService)
    {
        _dbContext = dbContext;
        _localizer = localizer;
        _webAuthnAuthenticationService = webAuthnAuthenticationService;
    }

    public async Task<IActionResult> Handle(WebAuthnAuthenticationAssertion request, CancellationToken cancellationToken)
    {
        var assertionUserId = await _webAuthnAuthenticationService
            .VerifyAuthenticationAssertionAsync(request.Challenge, request.Body.AssertionResponse);

        if (assertionUserId.IsFailure)
        {
            return new ErrorMessageResult(_localizer["Failed to pass WebAuthn assertion."]).ToBadRequest();
        }

        var userId = assertionUserId.Unwrap();

        var user = await _dbContext.Users
            .FirstAsync(x => x.EntityId == userId, cancellationToken);

        return new SignInResult(user.CreateIdentityClaimsPrincipal());
    }
}
