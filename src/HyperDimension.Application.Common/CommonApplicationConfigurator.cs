﻿using FluentValidation;
using HyperDimension.Application.Common.Behavior;
using HyperDimension.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0305

namespace HyperDimension.Application.Common;

public static class CommonApplicationConfigurator
{
    public static void AddHyperDimensionApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(ApplicationConstants.ProjectAssemblies);
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblies(ApplicationConstants.ProjectAssemblies.ToArray());

            configure.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
            configure.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configure.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped<IUrlHelper>(x => {
            var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            var factory = x.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext!);
        });
    }
}
