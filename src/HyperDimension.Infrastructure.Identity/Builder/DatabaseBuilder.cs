using System.Text;
using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("Database", typeof(DatabaseProviderOptions))]
public class DatabaseBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema { get; private set; } = true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        CanAddSchema = false;
    }
}
