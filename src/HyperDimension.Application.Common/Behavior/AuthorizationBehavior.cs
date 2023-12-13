using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Application.Common.Behavior;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<IActionResult>
    where TResponse : IActionResult
{
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IPermissionService _permissionService;

    public AuthorizationBehavior(
        IHyperDimensionRequestContext requestContext,
        IPermissionService permissionService)
    {
        _requestContext = requestContext;
        _permissionService = permissionService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Get attributes
        var schemaAttribute = request.GetType()
            .GetAttribute<RequireAuthenticationAttribute>();
        var permissionAttribute = request.GetType()
            .GetAttribute<PermissionAttribute>();

        // No schema and permission required (allow anonymous)
        if (schemaAttribute is null && permissionAttribute is null)
        {
            return await next();
        }

        // Check authenticated
        if (_requestContext.IsAuthenticated is false)
        {
            return (TResponse)(IActionResult)new UnauthorizedResult();
        }

        // Check schema
        var schemas = schemaAttribute?.Schemas ?? [IdentityConstants.IdentitySchema];
        if (schemas.Contains(_requestContext.AuthenticationSchema) is false)
        {
            return (TResponse)(IActionResult)new ForbidResult();
        }

        // Get permission attribute
        var permission = permissionAttribute?.Permission;

        // No permission required
        if (permission is null)
        {
            return await next();
        }

        // Check permission
        var allowAccess = await _permissionService.AllowAccess(permission, _requestContext.UserId.ExpectNotNull());
        if (allowAccess is false)
        {
            return (TResponse)(IActionResult)new ForbidResult();
        }

        return await next();
    }
}
