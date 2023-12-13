using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Application.Core.Identity.TwoFactor.GetStatus;

[RequireAuthentication(IdentityConstants.IdentitySchema, IdentityConstants.TwoFactorSchema)]
public class GetTwoFactorStatus : IRequest<IActionResult>;

public class GetTwoFactorStatusHandler : IRequestHandler<GetTwoFactorStatus, IActionResult>
{
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IHyperDimensionDbContext _dbContext;

    public GetTwoFactorStatusHandler(IHyperDimensionRequestContext requestContext, IHyperDimensionDbContext dbContext)
    {
        _requestContext = requestContext;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(GetTwoFactorStatus request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.Totp)
            .FirstAsync(u => u.EntityId == _requestContext.UserId, cancellationToken);

        return new OkObjectResult(new GetTwoFactorStatusDto
        {
            EmailEnabled = user.TwoFactorEmailEnabled,
            TotpEnabled = user.TwoFactorTotpEnabled
        });
    }
}
