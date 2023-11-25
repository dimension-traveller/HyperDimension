using HyperDimension.Common.Attributes;
using HyperDimension.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace HyperDimension.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetOrThrow<T>(this IConfiguration configuration)
    {
        var option = configuration.Get<T>()
                     ?? throw new ConfigurationException($"Cannot get option {typeof(T).Name} from configuration");
        return option;
    }

    public static T GetOption<T>(this IConfiguration configuration)
    {
        return (T)configuration.GetOption(typeof(T));
    }

    public static object GetOption(this IConfiguration configuration, Type type, bool noDefault = false)
    {
        var sectionNameAttribute = type.GetAttribute<OptionSectionAttribute>()
                                   ?? throw new ConfigurationException($"OptionSectionAttribute is not defined for {type.Name}");
        var section = configuration.GetSection(sectionNameAttribute.SectionName);

        if (noDefault)
        {
            var option = section.Get(type)
                ?? throw new ConfigurationException($"Cannot get option {type.Name} from configuration");
            return option;
        }

        var instance = Activator.CreateInstance(type)
                       ?? throw new ConfigurationException($"Cannot create option instance for {type.Name}");
        section.Bind(instance);
        return instance;
    }
}
