using System.Security.Cryptography.X509Certificates;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Database.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HyperDimension.Infrastructure.Database;

public static class DatabaseConfigurator
{
    public static void AddHyperDimensionDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IHyperDimensionDbContext, HyperDimensionDbContext>();

        // Add database builder
        var databaseOptions = HyperDimensionConfiguration.Instance
            .GetOption<DatabaseOptions>();
        var databaseBuilderType = ApplicationConstants.ProjectAssemblies
            .Scan<IDatabaseBuilder>()
            .ForDatabase(databaseOptions.Type)
            .FirstOrDefault()
            .ExpectNotNull();
        services.AddSingleton(typeof(IDatabaseBuilder), databaseBuilderType);

        // Add data protection
        var builder = services.AddDataProtection()
            .SetApplicationName("HyperDimension")
            .PersistKeysToDbContext<HyperDimensionDbContext>();

        var dataProtectionOptions = HyperDimensionConfiguration.Instance
            .GetOption<DatabaseDataProtectionOptions>();

        if (dataProtectionOptions.EnableCertificate is false)
        {
            return;
        }

        builder.ProtectKeysWithCertificate(new X509Certificate2(
            dataProtectionOptions.Certificate.Path,
            dataProtectionOptions.Certificate.Password));
        builder.UnprotectKeysWithAnyCertificate(
            dataProtectionOptions.RotatedCertificates
                .Select(x => new X509Certificate2(x.Path, x.Password))
                .ToArray());
    }

    public static async Task InitializeDatabaseAsync(this AsyncServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<HyperDimensionDbContext>();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<HyperDimensionDbContext>>();

        var appliedMigrations = await dbContext.Database
            .GetAppliedMigrationsAsync()
            .ToListAsync();
        var pendingMigrations = await dbContext.Database
            .GetPendingMigrationsAsync()
            .ToListAsync();

        logger.LogDebug("Applied migrations: {AppliedMigrations}", appliedMigrations);
        logger.LogDebug("Pending migrations: {PendingMigrations}", pendingMigrations);

        if (pendingMigrations.Count != 0)
        {
            logger.LogInformation("Applying migrations...");
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Migrations applied");
        }
    }
}
