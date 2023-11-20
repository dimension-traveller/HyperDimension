using HyperDimension.Common.Attributes;
using HyperDimension.Common.Exceptions;
using HyperDimension.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HyperDimension.Common.Configuration;

public static class ConfigurationConfigurator
{
    public static void AddHyperDimensionConfiguration(this IConfigurationBuilder builder)
    {
        var configurationFilePath = Environment
            .GetEnvironmentVariable("HD_CONFIGURATION_FILE_PATH") ?? "appsettings.yaml";

        var configurationBuilder = new ConfigurationBuilder();

        if (configurationFilePath.EndsWith(".toml", StringComparison.InvariantCulture))
        {
            configurationBuilder.AddTomlFile(configurationFilePath);

            var fileName = configurationFilePath[..^5];
            configurationBuilder.AddTomlFile($"{fileName}.{Constants.RuntimeEnvironment}.toml", true);
        }
        else if (configurationFilePath.EndsWith(".json", StringComparison.InvariantCulture))
        {
            configurationBuilder.AddJsonFile(configurationFilePath);

            var fileName = configurationFilePath[..^5];
            configurationBuilder.AddJsonFile($"{fileName}.{Constants.RuntimeEnvironment}.json", true);
        }
        else if (configurationFilePath.EndsWith(".yaml", StringComparison.InvariantCulture))
        {
            configurationBuilder.AddYamlFile(configurationFilePath);

            var fileName = configurationFilePath[..^5];
            configurationBuilder.AddYamlFile($"{fileName}.{Constants.RuntimeEnvironment}.json", true);
        }

        var configuration = configurationBuilder.Build();

        builder.AddConfiguration(configuration);
    }

    public static void AddHyperDimensionOptions(this IServiceCollection services)
    {
        services.AddOptions();

        var optionTypes = Constants.ProjectAssemblies
            .Scan()
            .Where(x => x.HasAttribute<OptionSectionAttribute>());

        foreach (var type in optionTypes)
        {
            services.AddSingleton(type, sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var attribute = type.GetAttribute<OptionSectionAttribute>();
                var sectionName = attribute?.SectionName ??
                                  type.Name.Replace("Options", string.Empty);

                var option = configuration.GetSection(sectionName).Get(type);

                if (option is null)
                {
                    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger(type.AssemblyQualifiedName ?? type.Name);

                    logger.LogWarning("Option {OptionName} is null, create default", type.AssemblyQualifiedName);

                    var defaultOptions = Activator.CreateInstance(type);
                    if (defaultOptions is null)
                    {
                        logger.LogError("Cannot create default option {OptionName}", type.AssemblyQualifiedName);
                    }

                    throw new ConfigurationException($"Cannot create option {type.AssemblyQualifiedName}");
                }

                return option;
            });
        }
    }
}
