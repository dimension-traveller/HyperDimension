using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Constants;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Domain.Entities.Security;
using HyperDimension.Infrastructure.Database.Configuration;
using HyperDimension.Infrastructure.Database.Extensions;
using HyperDimension.Infrastructure.Database.Interceptors;
using HyperDimension.Infrastructure.Database.Options;
using MediatR;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
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
    private readonly IDatabaseBuilder _databaseBuilder;

    private static readonly ConcurrentStampInterceptor ConcurrentStampInterceptor = new();

    public HyperDimensionDbContext(
        IMediator mediator,
        ILogger<HyperDimensionDbContext> logger,
        DatabaseOptions databaseOptions,
        IDatabaseBuilder databaseBuilder)
    {
        _mediator = mediator;
        _logger = logger;
        _databaseOptions = databaseOptions;
        _databaseBuilder = databaseBuilder;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        _databaseBuilder.Build(optionsBuilder);

        optionsBuilder.AddInterceptors(ConcurrentStampInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyCommonConfigurations();

        var entityConfigurationTypes = ApplicationConstants.ProjectAssemblies
            .Scan(x => x.IsAssignableTo(typeof(IEntityTypeConfiguration<>)))
            .ForDatabase(_databaseOptions.Type)
            .ToList();

        var applyConfigurationMethodInfo = typeof(ModelBuilder)
            .GetMethods()
            .Single(x => x is { Name: nameof(ModelBuilder.ApplyConfiguration), IsGenericMethod: true });

        foreach (var type in entityConfigurationTypes)
        {
            var entityType = type.GetInterfaces()
                .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .GetGenericArguments()
                .Single();

            var applyConfigurationMethod = applyConfigurationMethodInfo.MakeGenericMethod(entityType);
            applyConfigurationMethod.Invoke(modelBuilder, [Activator.CreateInstance(type)]);
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
