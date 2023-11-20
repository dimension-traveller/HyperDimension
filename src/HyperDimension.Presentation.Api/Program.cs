using HyperDimension.Common.Configuration;
using HyperDimension.Common.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddHyperDimensionConfiguration();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddHyperDimensionOptions();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
