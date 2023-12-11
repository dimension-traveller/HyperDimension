using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace HyperDimension.Application.Common.Interfaces.Database;

public interface IHyperDimensionDbContext
{
    #region Identity

    public DbSet<ApiToken> ApiTokens { get; set; }
    public DbSet<ExternalProvider> ExternalProviders { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Totp> Totps { get; set; }
    public DbSet<TotpRecoveryCode> TotpRecoveryCodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WebAuthn> WebAuthnDevices { get; set; }

    #endregion

    #region Security

    public DbSet<Token> Tokens { get; set; }

    #endregion

    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
