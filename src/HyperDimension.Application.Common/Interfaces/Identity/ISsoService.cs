using System.Security.Claims;
using HyperDimension.Application.Common.Models;

namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface ISsoService
{
    public ExternalUserInfo ReadExternalUserInfo(string schema, ClaimsPrincipal principal);
}
