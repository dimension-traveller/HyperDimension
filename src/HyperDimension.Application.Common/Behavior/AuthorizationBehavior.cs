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

    public AuthorizationBehavior(IHyperDimensionRequestContext requestContext)
    {
        _requestContext = requestContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Get attributes
        var schemaAttribute = request.GetType()
            .GetAttribute<RequireAuthenticationAttribute>();
        var ownerAttribute = request.GetType()
            .GetAttribute<RequireOwnerAttribute>();

        // No schema and no owner permission required (allow anonymous)
        if (schemaAttribute is null && ownerAttribute is null)
        {
            return await next();
        }

        // Check authenticated
        if (_requestContext.IsAuthenticated is false)
        {
            return (TResponse)(IActionResult)new UnauthorizedResult();
        }

        // Check schema
        var schemas = schemaAttribute?.Schemas ?? [IdentityConstants.IdentitySchema, IdentityConstants.StaticTokenSchema];
        if (schemas.Contains(_requestContext.AuthenticationSchema) is false)
        {
            return (TResponse)(IActionResult)new ForbidResult();
        }

        // Check owner
        if (ownerAttribute is not null && _requestContext.IsOwner is not true)
        {
            return (TResponse)(IActionResult)new ForbidResult();
        }

        return await next();
    }
}
