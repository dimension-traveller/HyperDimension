using HyperDimension.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperDimension.Infrastructure.Database.Configuration.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(x => x.Username)
            .IsUnique();

        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .HasMany(x => x.ApiTokens)
            .WithOne(x => x.User);

        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users);

        builder
            .HasMany(x => x.ExternalProviders)
            .WithOne(x => x.User);

        builder
            .HasMany(x => x.WebAuthnDevices)
            .WithOne(x => x.User);

        builder
            .HasOne(x => x.Totp);
    }
}
