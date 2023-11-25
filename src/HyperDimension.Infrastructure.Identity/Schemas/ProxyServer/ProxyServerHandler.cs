using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyperDimension.Infrastructure.Identity.Schemas.ProxyServer;

public class ProxyServerHandler(IOptionsMonitor<ProxyServerSchemaOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<ProxyServerSchemaOptions>(options, logger, encoder)
{
    private static readonly AuthenticateResult FailedNotHeader = AuthenticateResult.Fail("Missing username or email header.");
    private static readonly AuthenticateResult FailedNoValue = AuthenticateResult.Fail("Missing username or email header values.");

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headers = Context.Request.Headers;
        var hasUsernameHeader = headers.TryGetValue(Options.UsernameHeader, out var username);
        var hasEmailHeader = headers.TryGetValue(Options.EmailHeader, out var email);

        if (!hasUsernameHeader || !hasEmailHeader)
        {
            return Task.FromResult(FailedNotHeader);
        }

        if (string.IsNullOrEmpty(username.ToString()) || string.IsNullOrEmpty(email.ToString()))
        {
            return Task.FromResult(FailedNoValue);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username.ToString()),
            new(ClaimTypes.Email, email.ToString())
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
