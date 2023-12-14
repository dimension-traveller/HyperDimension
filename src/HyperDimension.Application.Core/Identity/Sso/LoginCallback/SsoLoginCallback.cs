using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.Sso.LoginCallback;

[RequireAuthentication(IdentityConstants.ExternalSchema)]
public class SsoLoginCallback : IRequest<IActionResult>
{
    [FromQuery(Name = "return_url")]
    public string? ReturnUrl { get; set; }
}

public class SsoLoginCallbackHandler : IRequestHandler<SsoLoginCallback, IActionResult>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
    private readonly ISsoService _ssoService;
    private readonly IStringLocalizer<SsoLoginCallbackHandler> _localizer;

    public SsoLoginCallbackHandler(
        IHttpContextAccessor httpContextAccessor,
        IAuthenticationSchemeProvider authenticationSchemeProvider,
        ISsoService ssoService,
        IStringLocalizer<SsoLoginCallbackHandler> localizer)
    {
        _httpContextAccessor = httpContextAccessor;
        _authenticationSchemeProvider = authenticationSchemeProvider;
        _ssoService = ssoService;
        _localizer = localizer;
    }

    public async Task<IActionResult> Handle(SsoLoginCallback request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext.ExpectNotNull();
        var auth = await context.AuthenticateAsync(IdentityConstants.ExternalSchema);
        var items = auth.Properties?.Items;
        var schema = string.Empty;
        var canGetSchema = items?.TryGetValue("SSO:Schema", out schema);

        if (auth.Succeeded is false)
        {
            var schemaDisplayName = "UNKNOWN";
            if (canGetSchema is true && string.IsNullOrEmpty(schema) is false)
            {
                var authenticationSchema = await _authenticationSchemeProvider
                    .GetSchemeAsync(schema);
                schemaDisplayName = authenticationSchema?.DisplayName ?? schema;
            }

            return new ErrorMessageResult(_localizer["Failed to login with external login provider {0}"].Format(schemaDisplayName)).ToBadRequest();
        }

        schema = schema.ExpectNotNull();

        var externalUserInfo = _ssoService.ReadExternalUserInfo(schema, auth.Principal);

        var properties = new AuthenticationProperties
        {
            RedirectUri = request.ReturnUrl,
            Items =
            {
                ["SSO:Schema"] = schema,
            }
        };

        var applicationPrinciple = externalUserInfo.CreateApplicationClaimsPrincipal();

        await context.SignInAsync(IdentityConstants.ApplicationSchema, applicationPrinciple, properties);
        return new SignInResult(IdentityConstants.ApplicationSchema, applicationPrinciple, properties);
    }
}
