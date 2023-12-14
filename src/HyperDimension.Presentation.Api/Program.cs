using HyperDimension.Application.Common;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Observability;
using HyperDimension.Presentation.Api.Formatter;
using HyperDimension.Presentation.Common;
using HyperDimension.Presentation.Common.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddHyperDimensionConfiguration(args);

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Insert(0, new HyperDimensionApiOutputFormatter());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHyperDimensionSwagger();

builder.Services.AddHyperDimensionCommon();
builder.Services.AddHyperDimensionInfrastructure();
builder.Services.AddHyperDimensionApplication();
builder.Services.AddHyperDimensionPresentation();

var app = builder.Build();

await app.InitializeAsync();

app.UseHyperDimensionSwagger();

app.UseAuthentication();

app.UseRequestContext();

app.MapControllers();

app.Run();
