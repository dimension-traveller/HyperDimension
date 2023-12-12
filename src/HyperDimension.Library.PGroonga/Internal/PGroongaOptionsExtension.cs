using HyperDimension.Library.PGroonga.ExpressionTranslators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

// ReSharper disable once CheckNamespace
namespace Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

public class PGroongaOptionsExtension : IDbContextOptionsExtension
{
    public DbContextOptionsExtensionInfo Info => new PGroongaExtensionInfo(this);

    public void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkPGroonga();
    }

    public void Validate(IDbContextOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
        if (internalServiceProvider is null)
        {
            return;
        }

        using var scope = internalServiceProvider.CreateScope();
        var methodCallTranslatorPlugins = scope.ServiceProvider.GetService<IEnumerable<IMethodCallTranslatorPlugin>>();
        var isRegistered = methodCallTranslatorPlugins?.Any(s => s is PGroongaMethodCallTranslatorPlugin) == true;
        if (isRegistered is false)
        {
            throw new InvalidOperationException($"{nameof(PGroongaConfigurator.UsePGroonga)} requires {nameof(PGroongaConfigurator.AddEntityFrameworkPGroonga)} to be called on the internal service provider used.");
        }
    }
}
