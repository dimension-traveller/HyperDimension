using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Common;

public static class CommonConfigurator
{
    public static void AddHyperDimensionCommonService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddPortableObjectLocalization(options =>
        {
            options.ResourcesPath = "Resources/Localization";
        });
    }
}
