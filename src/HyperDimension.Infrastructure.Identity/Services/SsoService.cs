using System.Security.Claims;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Infrastructure.Identity.Exceptions;
using HyperDimension.Infrastructure.Identity.Options;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity.Services;

public class SsoService : ISsoService
{
    private readonly IServiceProvider _serviceProvider;

    public SsoService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ExternalUserInfo ReadExternalUserInfo(string schema, ClaimsPrincipal principal)
    {
        var claimTypes = _serviceProvider.GetRequiredKeyedService<ExternalClaimsOptions>(schema);

        var username = principal.FindFirstValue(claimTypes.Username)
                       ?? throw new EmptyExternalClaimsException(schema, nameof(claimTypes.Username), claimTypes.Username);
        var displayName = principal.FindFirstValue(claimTypes.DisplayName)
                          ?? throw new EmptyExternalClaimsException(schema, nameof(claimTypes.DisplayName), claimTypes.DisplayName);
        var email = principal.FindFirstValue(claimTypes.Email)
                    ?? throw new EmptyExternalClaimsException(schema, nameof(claimTypes.Email), claimTypes.Email);
        var uniqueId = principal.FindFirstValue(claimTypes.UniqueId)
                       ?? throw new EmptyExternalClaimsException(schema, nameof(claimTypes.UniqueId), claimTypes.UniqueId);

        return new ExternalUserInfo
        {
            UniqueId = uniqueId,
            Username = username,
            DisplayName = displayName,
            Email = email,
            Schema = schema
        };
    }
}
