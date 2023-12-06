using FluentValidation;
using HyperDimension.Common;
using HyperDimension.Common.Constants;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0305

namespace HyperDimension.Application.Common;

public static class CommonApplicationConfigurator
{
    public static void AddCommonApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(ApplicationConstants.ProjectAssemblies);
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblies(ApplicationConstants.ProjectAssemblies.ToArray());
        });
    }
}
