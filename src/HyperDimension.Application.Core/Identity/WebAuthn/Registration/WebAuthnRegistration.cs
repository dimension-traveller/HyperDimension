using System.Text;
using System.Text.Json.Serialization;
using Fido2NetLib;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.WebAuthn.Registration;

public class WebAuthnRegistration : IRequest<IActionResult>
{
    [FromBody]
    public WebAuthnRegistrationBody Body { get; set; } = new();
}

public class WebAuthnRegistrationBody
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

public class WebAuthnRegistrationHandler : IRequestHandler<WebAuthnRegistration, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IStringLocalizer<WebAuthnRegistrationHandler> _localizer;
    private readonly IWebAuthnAuthenticationService _webAuthnAuthenticationService;

    public WebAuthnRegistrationHandler(
        IHyperDimensionDbContext dbContext,
        IHyperDimensionRequestContext requestContext,
        IStringLocalizer<WebAuthnRegistrationHandler> localizer,
        IWebAuthnAuthenticationService webAuthnAuthenticationService)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _localizer = localizer;
        _webAuthnAuthenticationService = webAuthnAuthenticationService;
    }

    public async Task<IActionResult> Handle(WebAuthnRegistration request, CancellationToken cancellationToken)
    {
        var fido2User = new Fido2User
        {
            Name = request.Body.Username,
            DisplayName = request.Body.DisplayName,
            Id = Encoding.UTF8.GetBytes(request.Body.Email)
        };

        var existingUser = await _dbContext.Users
            .Include(x => x.WebAuthnDevices)
            .FirstOrDefaultAsync(x => x.Email == request.Body.Email, cancellationToken);

        List<byte[]> existingCredentialIds = [];

        if (existingUser is not null)
        {
            var isAuthenticated = _requestContext.IsAuthenticated && existingUser.EntityId == _requestContext.UserId;

            if (isAuthenticated is false)
            {
                return new ErrorMessageResult(_localizer["User with the same email already exists."]).ToBadRequest();
            }

            existingCredentialIds = existingUser.WebAuthnDevices
                .Select(x => x.CredentialId)
                .ToList();
        }
        else
        {
            var usernameCollision = await _dbContext.Users
                .AnyAsync(x => x.Username == request.Body.Username, cancellationToken);
            if (usernameCollision)
            {
                return new ErrorMessageResult(_localizer["User with the same username already exists."]).ToBadRequest();
            }
        }

        var registrationOptions = await _webAuthnAuthenticationService
            .CreateRegistrationOptionsAsync(fido2User, existingCredentialIds);

        return new OkObjectResult(registrationOptions);
    }
}
