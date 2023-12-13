using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Utilities;
using HyperDimension.Domain.Entities.Security;
using HyperDimension.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Identity.Services;

public class SecurityTokenService : ISecurityTokenService
{
    private readonly IHyperDimensionDbContext _dbContext;

    public SecurityTokenService(IHyperDimensionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Token>> CreateTokenAsync(
        Guid userId, TokenUsage usage, DateTimeOffset currentTime,
        CancellationToken cancellationToken = default)
    {
        // Get existing token
        var existingToken = await _dbContext.Tokens
            .Where(x => x.BindTo == userId)
            .Where(x => x.Usage == usage)
            .Where(x => x.ExpiredAt >= currentTime)
            .ToListAsync(cancellationToken);

        // Check if tokens are out of time window
        var outOfTimeWindow = existingToken
            .Where(x => x.CreatedAt.AddSeconds(SecurityTokenConstants.TokenLifetimeWindow) < currentTime)
            .ToList();

        // If there are any out of time window token, remove them
        if (outOfTimeWindow.Count != 0)
        {
            _dbContext.Tokens.RemoveRange(outOfTimeWindow);
        }

        // Create new token
        var token = GenerateToken(usage);
        var tokenEntity = new Token
        {
            Value = token,
            BindTo = userId,
            Usage = usage,
            ExpiredAt = currentTime.AddSeconds(GetTokenLifetime(usage)),
            CreatedAt = currentTime
        };

        await _dbContext.Tokens.AddAsync(tokenEntity, cancellationToken);

        return token;
    }

    private static string GenerateToken(TokenUsage usage)
    {
        return usage switch
        {
            TokenUsage.AccountVerification => RandomUtility.GenerateToken(SecurityTokenConstants.AccountVerificationTokenLength),
            TokenUsage.EmailTwoFactorAuthentication => RandomUtility.GenerateNumericToken(SecurityTokenConstants.TwoFactorAuthenticationTokenLength),
            TokenUsage.PasswordReset => RandomUtility.GenerateToken(SecurityTokenConstants.PasswordResetTokenLength),
            _ => throw new ArgumentOutOfRangeException(nameof(usage), usage, null)
        };
    }

    private static int GetTokenLifetime(TokenUsage usage)
    {
        return usage switch
        {
            TokenUsage.AccountVerification => SecurityTokenConstants.AccountVerificationTokenLifetime,
            TokenUsage.EmailTwoFactorAuthentication => SecurityTokenConstants.TwoFactorAuthenticationTokenLifetime,
            TokenUsage.PasswordReset => SecurityTokenConstants.PasswordResetTokenLifetime,
            _ => throw new ArgumentOutOfRangeException(nameof(usage), usage, null)
        };
    }
}
