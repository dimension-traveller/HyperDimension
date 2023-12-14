using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.Sso.LoginConfirm;

[RequireAuthentication(IdentityConstants.ApplicationSchema)]
public class SsoLoginConfirm : IRequest<IActionResult>;

public class SsoLoginConfirmHandler : IRequestHandler<SsoLoginConfirm, IActionResult>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStringLocalizer<SsoLoginConfirmHandler> _localizer;
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
    private readonly IHyperDimensionDbContext _dbContext;

    public SsoLoginConfirmHandler(
        IHttpContextAccessor httpContextAccessor,
        IStringLocalizer<SsoLoginConfirmHandler> localizer,
        IAuthenticationSchemeProvider authenticationSchemeProvider,
        IHyperDimensionDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _localizer = localizer;
        _authenticationSchemeProvider = authenticationSchemeProvider;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(SsoLoginConfirm request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext.ExpectNotNull();
        var auth = await context.AuthenticateAsync(IdentityConstants.ApplicationSchema);
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

        var externalUserInfo = auth.Principal.ParseExternalUserInfo();

        // Find user by UniqueId
        var existingUserByUniqueId = await _dbContext.Users
            .Include(x => x.ExternalProviders)
            .Where(x => x.ExternalProviders
                .Exists(e => e.ProviderId == externalUserInfo.Schema && e.UserIdentifier == externalUserInfo.UniqueId))
            .FirstOrDefaultAsync(cancellationToken);
        if (existingUserByUniqueId is not null)
        {
            existingUserByUniqueId.Username = externalUserInfo.Username;
            existingUserByUniqueId.Email = externalUserInfo.Email;
            existingUserByUniqueId.DisplayName = externalUserInfo.DisplayName;
            existingUserByUniqueId.EmailConfirmed = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new SignInResult(existingUserByUniqueId.CreateIdentityClaimsPrincipal());
        }

        // Find user by email
        var existingUserByEmail = await _dbContext.Users
            .Include(x => x.ExternalProviders)
            .Where(x => x.Email == externalUserInfo.Email)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingUserByEmail is not null)
        {
            existingUserByEmail.ExternalProviders.Add(new ExternalProvider
            {
                ProviderId = externalUserInfo.Schema,
                UserIdentifier = externalUserInfo.UniqueId
            });

            existingUserByEmail.Username = externalUserInfo.Username;
            existingUserByEmail.Email = externalUserInfo.Email;
            existingUserByEmail.DisplayName = externalUserInfo.DisplayName;
            existingUserByEmail.EmailConfirmed = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            await context.SignInAsync(existingUserByEmail.CreateIdentityClaimsPrincipal());
            return new SignInResult(existingUserByEmail.CreateIdentityClaimsPrincipal());
        }

        // Create new user
        var user = new User
        {
            Username = externalUserInfo.Username,
            Email = externalUserInfo.Email,
            DisplayName = externalUserInfo.DisplayName,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            ExternalProviders =
            [
                new ExternalProvider
                {
                    ProviderId = externalUserInfo.Schema,
                    UserIdentifier = externalUserInfo.UniqueId
                }
            ]
        };
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SignInResult(user.CreateIdentityClaimsPrincipal());
    }
}
