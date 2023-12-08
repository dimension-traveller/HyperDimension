using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Presentation.Common.Middlewares;
using HyperDimension.Presentation.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Presentation.Common;

public static class CommonPresentationConfigurator
{
    public static void AddHyperDimensionPresentation(this IServiceCollection services)
    {
        services.AddScoped<IHyperDimensionRequestContext, HyperDimensionRequestContext>();
    }

    public static void UseRequestContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextMiddleware>();
    }
}
