using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.UserManagement.PasswordReset;

public class PasswordReset : IRequest<IActionResult>
{
    [FromBody]
    public PasswordResetBody Body { get; set; } = new();
}

public class PasswordResetBody
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class PasswordResetHandler : IRequestHandler<PasswordReset, IActionResult>
{
    private readonly IPasswordHashService _passwordHashService;
    private readonly IStringLocalizer<PasswordResetHandler> _localizer;
    private readonly IHyperDimensionDbContext _dbContext;

    public PasswordResetHandler(
        IPasswordHashService passwordHashService,
        IStringLocalizer<PasswordResetHandler> localizer,
        IHyperDimensionDbContext dbContext)
    {
        _passwordHashService = passwordHashService;
        _localizer = localizer;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(PasswordReset request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var token = await _dbContext.Tokens
            .Where(x => x.ExpiredAt >= now)
            .Where(x => x.Usage == TokenUsage.PasswordReset)
            .Where(x => x.Value == request.Body.Token)
            .FirstOrDefaultAsync(cancellationToken);

        if (token is null)
        {
            return new ErrorMessageResult(_localizer["Invalid password reset token."]).ToBadRequest();
        }

        var user = await _dbContext.Users
            .Where(x => x.EntityId == token.BindTo)
            .FirstAsync(cancellationToken);

        var securityStamp = Guid.NewGuid().ToString();
        var passwordHash = _passwordHashService.HashPassword(request.Body.Password, securityStamp);
        user.SecurityStamp = securityStamp;
        user.PasswordHash = passwordHash;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new NoContentResult();
    }
}

