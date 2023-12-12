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
        // Check anonymous
        var allowAnonymous = request.GetType()
            .GetAttribute<AllowAnonymousAttribute>();
        if (allowAnonymous is not null)
        {
            return await next();
        }

        // Check authenticated
        if (_requestContext.IsAuthenticated is false)
        {
            return (TResponse)(IActionResult)new UnauthorizedResult();
        }

        // Check schema
        var schema = request.GetType()
            .GetAttribute<IdentitySchemaAttribute>()?.Schema
                     ?? IdentityConstants.IdentitySchema;
        if (_requestContext.AuthenticationSchema != schema)
        {
            return (TResponse)(IActionResult)new ForbidResult();
        }

        // Get permission attribute
        var permission = request.GetType()
            .GetAttribute<PermissionAttribute>()?.Permission;

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
