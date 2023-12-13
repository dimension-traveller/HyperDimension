using Fido2NetLib;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Exceptions;
using HyperDimension.Infrastructure.Identity.Options;
using HyperDimension.Infrastructure.Identity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0305

namespace HyperDimension.Infrastructure.Identity;

public static class IdentityConfigurator
{
    public static void AddHyperDimensionIdentity(this IServiceCollection services)
    {
        services.AddHyperDimensionAuthentication();

        services.AddSingleton<IPasswordHashService, PasswordHashService>();
        services.AddSingleton<ITotpService, TotpService>();
        services.AddScoped<ISecurityTokenService, SecurityTokenService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IWebAuthnAuthenticationService, WebAuthnAuthenticationService>();

        services.AddSingleton<IFido2, Fido2>(sp =>
        {
            var options = sp.GetRequiredService<WebAuthnOptions>();

            var fido2Configuration = new Fido2Configuration
            {
                Timeout = options.Timeout,
                ServerIcon = options.ServerIcon,
                ServerDomain = options.ServerDomain,
                ServerName = options.ServerName,
                Origins = options.Origins.ToHashSet()
            };

            return new Fido2(fido2Configuration);
        });
    }

    private static void AddHyperDimensionAuthentication(this IServiceCollection services)
    {
        var identityOptions = HyperDimensionConfiguration.Instance
            .GetOption<HdIdentityOptions>();

        // Do not allow duplicate ids or names
        var duplicateIds = identityOptions.Providers
            .GroupBy(x => x.Id)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();
        var duplicateNames = identityOptions.Providers
            .GroupBy(x => x.Name)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();
        if (duplicateIds.Length != 0)
        {
            throw new DuplicatedAuthenticationSchemaException("Id", duplicateIds);
        }
        if (duplicateNames.Length != 0)
        {
            throw new DuplicatedAuthenticationSchemaException("Name", duplicateNames);
        }

        var builder = services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationSchema;
                options.DefaultSignInScheme = IdentityConstants.ExternalSchema;
            })
            .AddBearerToken(IdentityConstants.IdentitySchema, options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromSeconds(identityOptions.Token.AccessTokenExpiration);
                options.RefreshTokenExpiration = TimeSpan.FromSeconds(identityOptions.Token.RefreshTokenExpiration);
            })
            .AddBearerToken(IdentityConstants.TwoFactorSchema, options =>
            {
                options.BearerTokenExpiration = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.ApplicationSchema, "Application", options =>
            {
                options.Cookie.Name = IdentityConstants.ApplicationSchema;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.ExternalSchema, "External", options =>
            {
                options.Cookie.Name = IdentityConstants.ExternalSchema;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

        var availableBuilder = ApplicationConstants.ProjectAssemblies
            .Scan()
            .Where(x => x.HasAttribute<AuthenticationBuilderAttribute>())
            .ToDictionary(
                x => x.GetAttribute<AuthenticationBuilderAttribute>()!.Name,
                y => Activator.CreateInstance(y)!);

        var index = 0;
        foreach (var provider in identityOptions.Providers)
        {
            if (availableBuilder.TryGetValue(provider.Type, out var instance) is false)
            {
                throw new AuthenticationNotSupportedException(provider.Type, "Unknown authentication type");
            }

            var canAddSchema = (bool) instance.GetType()
                .GetProperty("CanAddSchema")!
                .GetValue(instance)!;

            if (canAddSchema is false)
            {
                throw new AuthenticationNotSupportedException(provider.Type, "This authentication type does not support adding more schemas");
            }

            var configType = instance.GetType()
                .GetInterfaces()
                .First(x => x.Name == "IAuthenticationProviderBuilder`1")
                .GetGenericArguments()[0];
            var config = Activator.CreateInstance(configType)!;
            HyperDimensionConfiguration.Instance
                .GetSection($"Identity:Providers:{index}:Config").Bind(config);
            index++;

            instance.GetType()
                .GetMethod("AddSchema")!
                .Invoke(instance, new[] { builder, provider, config });
        }
    }
}
