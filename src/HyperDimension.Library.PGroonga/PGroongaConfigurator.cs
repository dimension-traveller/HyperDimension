using HyperDimension.Library.PGroonga.ExpressionTranslators;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

public static class PGroongaConfigurator
{
    public static NpgsqlDbContextOptionsBuilder UsePGroonga(this NpgsqlDbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var coreOptionsBuilder = ((IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder)
            .OptionsBuilder;

        var extension = coreOptionsBuilder.Options.FindExtension<PGroongaOptionsExtension>()
                        ?? new PGroongaOptionsExtension();

        ((IDbContextOptionsBuilderInfrastructure)coreOptionsBuilder)
            .AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    public static IServiceCollection AddEntityFrameworkPGroonga(this IServiceCollection serviceCollection)
    {
        new EntityFrameworkRelationalServicesBuilder(serviceCollection)
            .TryAdd<IMethodCallTranslatorPlugin, PGroongaMethodCallTranslatorPlugin>()
            .TryAdd<IEvaluatableExpressionFilterPlugin, PGroongaEvaluatableExpressionFilterPlugin>();

        return serviceCollection;
    }
}
