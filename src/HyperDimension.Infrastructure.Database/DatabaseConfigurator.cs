using HyperDimension.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Database;

public static class DatabaseConfigurator
{
    public static void AddHyperDimensionDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IHyperDimensionDbContext, HyperDimensionDbContext>();
    }
}
