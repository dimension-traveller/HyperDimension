using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("Database")]
public class DatabaseBuilder : IAuthenticationProviderBuilder<DatabaseProviderOptions>
{
    public bool CanAddSchema { get; private set; } = true;

    public void AddSchema(AuthenticationBuilder builder, IdentityProviderOptions metadata, DatabaseProviderOptions options)
    {
        CanAddSchema = false;
    }
}
