using HyperDimension.Application.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Api.Controllers.Identity;

[ApiController]
[Route("identity/sso")]
public class ExternalLoginController(
    IAuthenticationSchemeProvider authenticationSchemeProvider) : ControllerBase
{
    [HttpGet("{schema}")]
    public async Task<IActionResult> Login([FromRoute] string schema, [FromQuery(Name = "redirect")] string? redirect = null)
    {
        var authenticationSchema = await authenticationSchemeProvider
            .GetSchemeAsync(schema);

        if (authenticationSchema is not null)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = string.IsNullOrEmpty(redirect) ? "/" : redirect
            }, [schema]);
        }

        var allSchemas = await authenticationSchemeProvider.GetAllSchemesAsync();
        var schemaNames = allSchemas
            .Where(x => x.HandlerType != typeof(CookieAuthenticationHandler))
            .Select(x => $"{x.Name} ({x.DisplayName})");

        var schemaNamesString = string.Join(", ", schemaNames);

        return BadRequest(new ErrorMessageResult
        {
            Message = "Unknown SSO authentication schema, available schemas: " + schemaNamesString
        });
    }
}
