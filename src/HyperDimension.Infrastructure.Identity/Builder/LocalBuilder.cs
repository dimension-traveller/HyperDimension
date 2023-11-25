using System.Text;
using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Options.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HyperDimension.Infrastructure.Identity.Builder;

[AuthenticationBuilder("Local", typeof(LocalOptions))]
public class LocalBuilder : IAuthenticationProviderBuilder
{
    public bool CanAddSchema { get; private set; } = true;

    public void AddSchema(AuthenticationBuilder builder, string id, string name, object options)
    {
        var opt = (LocalOptions)options;

        builder
            .AddJwtBearer(id, name, o =>
            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = opt.Issuer,
                    ValidAudience = opt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Secret)),
                };
            });

        CanAddSchema = false;
    }
}
