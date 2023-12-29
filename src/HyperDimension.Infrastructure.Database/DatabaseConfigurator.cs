using System.Security.Cryptography.X509Certificates;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using HyperDimension.Common.Utilities;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Infrastructure.Database.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

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
        services.AddHealthChecks().AddDbContextCheck<HyperDimensionDbContext>("ef-core");

        // Tracing
        if (databaseOptions.Tracing)
        {
            services.ConfigureOpenTelemetryTracerProvider(configure =>
            {
                configure.AddEntityFrameworkCoreInstrumentation(options =>
                {
                    options.SetDbStatementForText = true;
                    options.SetDbStatementForStoredProcedure = true;
                });
            });
        }

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

        var owner = dbContext.Users.FirstOrDefault(x => x.IsOwner);
        if (owner is not null)
        {
            logger.LogDebug("Database initialized");
            return;
        }

        logger.LogWarning("No owner found, creating one...");

        var passwordHashService = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();

        var ownerUsername = Environment.GetEnvironmentVariable("OOBE_OWNER_USERNAME") ?? "admin";
        var ownerName = Environment.GetEnvironmentVariable("OOBE_OWNER_NAME") ?? "Admin";
        var ownerEmail = Environment.GetEnvironmentVariable("OOBE_OWNER_EMAIL") ?? "admin@example.com";
        var ownerPassword = Environment.GetEnvironmentVariable("OOBE_OWNER_PASSWORD") ?? RandomUtility.GenerateToken(16);

        logger.LogWarning("Owner username: {OwnerUsername}", ownerUsername);
        logger.LogWarning("Owner name: {OwnerName}", ownerName);
        logger.LogWarning("Owner email: {OwnerEmail}", ownerEmail);
        logger.LogWarning("Owner password: {OwnerPassword}", ownerPassword);
        logger.LogWarning("Please change the password immediately after first login");

        var stamp = Guid.NewGuid().ToString();

        var passwordHash = passwordHashService.HashPassword(ownerPassword, stamp);

        owner = new User
        {
            Username = ownerUsername,
            Email = ownerEmail,
            DisplayName = ownerName,
            PasswordHash = passwordHash,
            SecurityStamp = stamp,
            EmailConfirmed = true,
            IsOwner = true
        };

        dbContext.Users.Add(owner);

        logger.LogDebug("Database initialized");
    }
}
