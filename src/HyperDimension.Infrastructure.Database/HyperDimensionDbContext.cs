using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Common.Constants;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Infrastructure.Database.Configuration;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Exceptions;
using HyperDimension.Infrastructure.Database.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HyperDimension.Infrastructure.Database;

public class HyperDimensionDbContext(
    IMediator mediator,
    ILogger<HyperDimensionDbContext> logger,
    DatabaseOptions databaseOptions)
    : DbContext, IDataProtectionKeyContext, IHyperDimensionDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        switch (databaseOptions.Type)
        {
            case DatabaseType.SQLite:
                optionsBuilder.UseSqlite(databaseOptions.ConnectionString);
                break;
            case DatabaseType.SQLServer:
                optionsBuilder.UseSqlServer(databaseOptions.ConnectionString, options =>
                {
                    options.EnableRetryOnFailure(8);
                });
                break;
            case DatabaseType.MySQL:
                throw new DatabaseNotSupportedException(databaseOptions.Type.ToString(), "Waiting for Pomelo.EntityFrameworkCore.MySql 8.0.0");
            case DatabaseType.PostgreSQL:
                optionsBuilder.UseNpgsql(databaseOptions.ConnectionString, options =>
                {
                    options.EnableRetryOnFailure(8);
                });
                break;
            case DatabaseType.Oracle:
                optionsBuilder.UseOracle(databaseOptions.ConnectionString);
                break;
            default:
                throw new DatabaseNotSupportedException(databaseOptions.Type.ToString(), "Unknown database type");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyCommonConfigurations();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HyperDimensionDbContext).Assembly);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    #region Data Protection

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    #endregion

    #region Identity

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<WebAuthn> WebAuthnDevices { get; set; }

    #endregion

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await mediator.DispatchDomainEvents(this);

        if (ApplicationConstants.IsDevelopment)
        {
            logger.LogTrace("DbContext ChangeTracker (Long): {ChangeTrackerLongView}",
                ChangeTracker.DebugView.LongView);
            logger.LogDebug("DbContext ChangeTracker (Short): {ChangeTrackerShortView}",
                ChangeTracker.DebugView.ShortView);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
