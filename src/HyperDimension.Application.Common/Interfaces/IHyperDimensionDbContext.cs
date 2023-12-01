using HyperDimension.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionDbContext
{
    #region Identity

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<WebAuthn> WebAuthnDevices { get; set; }

    #endregion

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
