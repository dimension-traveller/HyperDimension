using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Infrastructure.Database.Configuration;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Exceptions;
using HyperDimension.Infrastructure.Database.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HyperDimension.Infrastructure.Database;

public class HyperDimensionDbContext(
    IMediator mediator,
    ILogger<HyperDimensionDbContext> logger,
    DatabaseOptions databaseOptions)
    : DbContext, IHyperDimensionDbContext
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await mediator.DispatchDomainEvents(this);

        if (Constants.IsDevelopment)
        {
            logger.LogTrace("DbContext ChangeTracker (Long): {ChangeTrackerLongView}",
                ChangeTracker.DebugView.LongView);
            logger.LogDebug("DbContext ChangeTracker (Short): {ChangeTrackerShortView}",
                ChangeTracker.DebugView.ShortView);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
