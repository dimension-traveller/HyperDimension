using HyperDimension.Common.Configuration;
using HyperDimension.Common.Observability;
using HyperDimension.Infrastructure.Database;
using HyperDimension.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddHyperDimensionConfiguration();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddHyperDimensionOptions();
builder.Services.AddHyperDimensionDatabase();
builder.Services.AddHyperDimensionStorage();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
