using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Domain.Entities.Security;
using HyperDimension.Infrastructure.Database.Configuration;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Exceptions;
using HyperDimension.Infrastructure.Database.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace HyperDimension.Infrastructure.Database;

public class HyperDimensionDbContext
    : DbContext, IDataProtectionKeyContext, IHyperDimensionDbContext
{
    private readonly IMediator _mediator;
    private readonly ILogger<HyperDimensionDbContext> _logger;
    private readonly DatabaseOptions _databaseOptions;

    public HyperDimensionDbContext(
        IMediator mediator,
        ILogger<HyperDimensionDbContext> logger,
        DatabaseOptions databaseOptions)
    {
        _mediator = mediator;
        _logger = logger;
        _databaseOptions = databaseOptions;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        switch (_databaseOptions.Type)
        {
            case DatabaseType.SQLite:
                optionsBuilder.UseSqlite(_databaseOptions.ConnectionString);
                var sqliteDataSource = new SqliteConnectionStringBuilder(_databaseOptions.ConnectionString).DataSource;
                if (string.Equals(sqliteDataSource, ":memory:", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                sqliteDataSource.EnsureFileExist();
                break;
            case DatabaseType.SQLServer:
                optionsBuilder.UseSqlServer(_databaseOptions.ConnectionString, options =>
                {
                    options.EnableRetryOnFailure(8);
                });
                break;
            case DatabaseType.MySQL:
                throw new DatabaseNotSupportedException(_databaseOptions.Type.ToString(), "Waiting for Pomelo.EntityFrameworkCore.MySql 8.0.0");
            case DatabaseType.PostgreSQL:
                optionsBuilder.UseNpgsql(_databaseOptions.ConnectionString, options =>
                {
                    options.EnableRetryOnFailure(8);
                });
                break;
            case DatabaseType.Oracle:
                optionsBuilder.UseOracle(_databaseOptions.ConnectionString);
                break;
            default:
                throw new DatabaseNotSupportedException(_databaseOptions.Type.ToString(), "Unknown database type");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyCommonConfigurations();

        foreach (var projectAssembly in ApplicationConstants.ProjectAssemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(projectAssembly);
        }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    #region Data Protection

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    #endregion

    #region Identity

    public DbSet<TotpRecoveryCode> TotpRecoveryCodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ApiToken> ApiTokens { get; set; }
    public DbSet<ExternalProvider> ExternalProviders { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Totp> Totps { get; set; }
    public DbSet<WebAuthn> WebAuthnDevices { get; set; }

    #endregion

    #region Security

    public DbSet<Token> Tokens { get; set; }

    #endregion

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await _mediator.DispatchDomainEvents(this);

        if (ApplicationConstants.IsDevelopment)
        {
            _logger.LogTrace("DbContext ChangeTracker (Long): {ChangeTrackerLongView}",
                ChangeTracker.DebugView.LongView);
            _logger.LogDebug("DbContext ChangeTracker (Short): {ChangeTrackerShortView}",
                ChangeTracker.DebugView.ShortView);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
