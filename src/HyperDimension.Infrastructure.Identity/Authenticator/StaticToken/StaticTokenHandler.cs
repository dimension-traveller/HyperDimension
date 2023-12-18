using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyperDimension.Infrastructure.Identity.Authenticator.StaticToken;

public class StaticTokenHandler : AuthenticationHandler<StaticTokenOptions>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IDistributedCache _cache;


    [Obsolete("Obsolete")]
    [SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed")]
    public StaticTokenHandler(
        IHyperDimensionDbContext dbContext,
        IDistributedCache cache,
        IOptionsMonitor<StaticTokenOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public StaticTokenHandler(
        IHyperDimensionDbContext dbContext,
        IDistributedCache cache,
        IOptionsMonitor<StaticTokenOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.Fail("Authorization header is not found.");
        }

        var token = authorizationHeader.Split(' ');
        if (token.Length != 2)
        {
            return AuthenticateResult.Fail("Invalid token format.");
        }

        var tokenSchema = token[0];
        if (tokenSchema.Equals("Bearer", StringComparison.OrdinalIgnoreCase) is false)
        {
            return AuthenticateResult.Fail("Invalid token schema.");
        }

        var tokenValue = token[1];
        var cacheKey = $"api-token:auth:{tokenValue}";
        var userIdentifier = await _cache.GetStringAsync(cacheKey);
        var userIdValid = Guid.TryParse(userIdentifier, out var userId);
        if (userIdValid)
        {
            var user = await _dbContext.Users
                .Include(x => x.ApiTokens)
                .FirstAsync(x => x.EntityId == userId);
            user.ApiTokens.First(x => x.Token == tokenValue).LastUsedAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            var cachedPrinciple = user.CreateIdentityClaimsPrincipal();
            var cachedTicket = new AuthenticationTicket(cachedPrinciple, Scheme.Name);
            return AuthenticateResult.Success(cachedTicket);
        }

        var apiToken = await _dbContext.ApiTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == tokenValue);

        if (apiToken is null)
        {
            return AuthenticateResult.Fail("Invalid token.");
        }

        await _cache.SetStringAsync(cacheKey, apiToken.User.EntityId.ToString(), 60 * 60);

        apiToken.LastUsedAt = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var principle = apiToken.User.CreateIdentityClaimsPrincipal();
        var ticket = new AuthenticationTicket(principle, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}
