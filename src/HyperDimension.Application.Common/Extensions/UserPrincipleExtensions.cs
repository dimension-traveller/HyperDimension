using System.Security.Claims;
using HyperDimension.Common.Constants;
using HyperDimension.Domain.Entities.Identity;

namespace HyperDimension.Application.Common.Extensions;

public static class UserPrincipleExtensions
{
    private const string IdClaimType = "sub";
    private const string TwoFactorClaimType = "amr";

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
