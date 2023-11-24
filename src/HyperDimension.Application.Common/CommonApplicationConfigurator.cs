using FluentValidation;
using HyperDimension.Common;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Application.Common;

public static class CommonApplicationConfigurator
{
    public static void AddCommonApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(Constants.ProjectAssemblies);
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblies([.. Constants.ProjectAssemblies]);
        });
    }
}
