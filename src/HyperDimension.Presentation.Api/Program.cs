using HyperDimension.Application.Common;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Observability;
using HyperDimension.Infrastructure.Cache;
using HyperDimension.Infrastructure.Database;
using HyperDimension.Infrastructure.Identity;
using HyperDimension.Infrastructure.Storage;
using HyperDimension.Presentation.Api.Formatter;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddHyperDimensionConfiguration();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Insert(0, new HyperDimensionApiOutputFormatter());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHyperDimensionOptions();
builder.Services.AddHyperDimensionDatabase();
builder.Services.AddHyperDimensionStorage();
builder.Services.AddHyperDimensionCache();
builder.Services.AddHyperDimensionIdentity();
builder.Services.AddCommonApplicationServices();

var app = builder.Build();

app.UseAuthentication();

app.MapControllers();

app.Run();
