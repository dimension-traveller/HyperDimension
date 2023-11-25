﻿using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Identity.Abstract;
using HyperDimension.Infrastructure.Identity.Attributes;
using HyperDimension.Infrastructure.Identity.Exceptions;
using HyperDimension.Infrastructure.Identity.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Identity;

public static class IdentityConfigurator
{
    public static void AddHyperDimensionIdentity(this IServiceCollection services)
    {
        services.AddHyperDimensionAuthentication();
    }

    private static void AddHyperDimensionAuthentication(this IServiceCollection services)
    {
        var defaultSchema = HyperDimensionConfiguration
            .Instance
            .GetRequiredSection("Identity:Default")
            .Value;

        var authenticationSection = HyperDimensionConfiguration
            .Instance
            .GetRequiredSection("Identity:Providers");
        var providersConfiguration = authenticationSection
            .GetChildren();
        var providers = providersConfiguration
            .Select(x => (
                IdentityOptions: x.GetOrThrow<IdentityOptions>(),
                ConfigurationSection: x.GetRequiredSection("Config")
                ))
            .ToArray();

        // Do not allow duplicate ids or names
        var duplicateIds = providers
            .GroupBy(x => x.IdentityOptions.Id)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();
        var duplicateNames = providers
            .GroupBy(x => x.IdentityOptions.Name)
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

        if (defaultSchema is null)
        {
            defaultSchema = providers[0].IdentityOptions.Id;
        }
        else
        {
            if (providers.Select(x => x.IdentityOptions.Id).Contains(defaultSchema) is false)
            {
                throw new AuthenticationNotSupportedException(defaultSchema, "Default schema not found");
            }
        }

        // Prepare the builder
        var builders = typeof(IdentityConfigurator).Assembly
            .Scan<IAuthenticationProviderBuilder>()
            .Where(x => x.HasAttribute<AuthenticationBuilderAttribute>())
            .Select(x => (
                Attribute: x.GetAttribute<AuthenticationBuilderAttribute>()!,
                Instance: (IAuthenticationProviderBuilder)Activator.CreateInstance(x)!))
            .ToDictionary(
                x => x.Attribute.Name,
                x => (
                    OptionType: x.Attribute.OptionsType,
                    x.Instance));

        var builder = services
            .AddAuthentication(defaultSchema);

        // Add the schema
        foreach (var (ido, section) in providers)
        {
            if (builders.TryGetValue(ido.Type, out var builderPair) is false)
            {
                throw new AuthenticationNotSupportedException(ido.Type, "Unknown authentication type");
            }

            var (optionType, instance) = builderPair;

            if (instance.CanAddSchema is false)
            {
                throw new AuthenticationNotSupportedException(ido.Type, $"Builder blocked schema {ido.Type}");
            }

            var options = Activator.CreateInstance(optionType)
                ?? throw new AuthenticationNotSupportedException(ido.Type, "Unable to create options instance");
            section.Bind(options);
            instance.AddSchema(builder, ido.Id, ido.Name, options);
        }
    }
}
