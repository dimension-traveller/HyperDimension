using System.Diagnostics.CodeAnalysis;
using HyperDimension.Common;
using HyperDimension.Common.Constants;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HyperDimension.Presentation.Common.Swagger;

[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded")]
public static class SwaggerConfigurator
{
    public static void AddHyperDimensionSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HyperDimension API",
                Version = "v1",
                Description = "HyperDimension backend API definitions",
                Contact = new OpenApiContact
                {
                    Name = "Alisa",
                    Email = "alisa@alisaqaq.moe",
                },
                License = new OpenApiLicense
                {
                    Name = "Gnu Affero General Public License v3.0",
                    Url = new Uri("https://www.gnu.org/licenses/agpl-3.0.en.html"),
                }
            });

            var filePath = Path.Combine(AppContext.BaseDirectory, "HyperDimension.Application.Core.xml");
            c.IncludeXmlComments(filePath);
        });

        services.AddFluentValidationRulesToSwagger();
    }

    public static void UseHyperDimensionSwagger(this IApplicationBuilder app)
    {
        if (ApplicationConstants.IsDevelopment is false)
        {
            return;
        }

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "HyperDimension API v1");
        });
    }
}
