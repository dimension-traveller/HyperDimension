using HyperDimension.Common;
using HyperDimension.Common.Configuration;
using HyperDimension.Infrastructure.Cache;
using HyperDimension.Infrastructure.Common;
using HyperDimension.Infrastructure.Database;
using HyperDimension.Infrastructure.Identity;
using HyperDimension.Infrastructure.Search;
using HyperDimension.Infrastructure.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Presentation.Common;

public static class ConfiguratorExtensions
{
    public static void AddHyperDimensionInfrastructure(this IServiceCollection services)
    {
        services.AddCommonInfrastructure();
        services.AddHyperDimensionCache();
        services.AddHyperDimensionDatabase();
        services.AddHyperDimensionStorage();
        services.AddHyperDimensionCache();
        services.AddHyperDimensionIdentity();
        services.AddHyperDimensionSearch();
    }

    public static void AddHyperDimensionCommon(this IServiceCollection services)
    {
        services.AddHyperDimensionCommonService();
        services.AddHyperDimensionOptions();
    }

    public static async Task InitializeAsync(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateAsyncScope();

        await scope.InitializeDatabaseAsync();
    }
}
