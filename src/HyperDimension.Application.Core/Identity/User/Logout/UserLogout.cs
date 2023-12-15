using HyperDimension.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Application.Core.Identity.User.Logout;

public class UserLogout : IRequest<IActionResult>;

public class UserLogoutHandler : IRequestHandler<UserLogout, IActionResult>
{
    public Task<IActionResult> Handle(UserLogout request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(new SignOutResult(IdentityConstants.IdentitySchema));
    }
}
