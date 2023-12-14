using Microsoft.Extensions.Configuration;

namespace HyperDimension.Common.Configuration;

public static class HyperDimensionConfiguration
{
    public static IConfiguration Instance { get; internal set; } = ConfigurationConfigurator.GetConfiguration();
}
