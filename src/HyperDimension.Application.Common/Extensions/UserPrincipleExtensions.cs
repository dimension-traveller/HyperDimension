using System.Security.Claims;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Constants;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Application.Common.Extensions;

public static class UserPrincipleExtensions
{
    private const string IdClaimType = "sub";
    private const string TwoFactorClaimType = "amr";

    private const string ExternalUniqueIdClaimType = "e_sub";
    private const string ExternalUsernameClaimType = "e_username";
    private const string ExternalDisplayNameClaimType = "e_name";
    private const string ExternalEmailClaimType = "e_email";
    private const string ExternalSchemaClaimType = "e_schema";

    public static ClaimsPrincipal CreateIdentityClaimsPrincipal(this User user)
    {
        var claims = new Claim[]
        {
            new(IdClaimType, user.EntityId.ToString()),
            new(TwoFactorClaimType, "true")
        };

        var identity = new ClaimsIdentity(claims, IdentityConstants.IdentitySchema);
        var principle = new ClaimsPrincipal(identity);

        return principle;
    }

    public static ClaimsPrincipal CreateApplicationClaimsPrincipal(this ExternalUserInfo user)
    {
        var claims = new Claim[]
        {
            new(ExternalUniqueIdClaimType, user.UniqueId),
            new(ExternalUsernameClaimType, user.Username),
            new(ExternalDisplayNameClaimType, user.DisplayName),
            new(ExternalEmailClaimType, user.Email),
            new(ExternalSchemaClaimType, user.Schema)
        };

        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationSchema);
        var principle = new ClaimsPrincipal(identity);

        return principle;
    }

    public static ExternalUserInfo ParseExternalUserInfo(this ClaimsPrincipal principal)
    {
        var uniqueId = principal.FindFirstValue(ExternalUniqueIdClaimType).ExpectNotNull();
        var username = principal.FindFirstValue(ExternalUsernameClaimType).ExpectNotNull();
        var displayName = principal.FindFirstValue(ExternalDisplayNameClaimType).ExpectNotNull();
        var email = principal.FindFirstValue(ExternalEmailClaimType).ExpectNotNull();
        var schema = principal.FindFirstValue(ExternalSchemaClaimType).ExpectNotNull();

        return new ExternalUserInfo
        {
            UniqueId = uniqueId,
            Username = username,
            DisplayName = displayName,
            Email = email,
            Schema = schema
        };
    }

    public static ClaimsPrincipal CreateTwoFactorClaimsPrincipal(this User user)
    {
        var claims = new Claim[]
        {
            new(IdClaimType, user.EntityId.ToString()),
            new(TwoFactorClaimType, "false")
        };

        var identity = new ClaimsIdentity(claims, IdentityConstants.TwoFactorSchema);
        var principle = new ClaimsPrincipal(identity);

        return principle;
    }

    public static Guid? GetUserEntityId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirst(IdClaimType)?.Value;
        var valid = Guid.TryParse(id, out var entityId);
        return valid ? entityId : null;
    }
}
