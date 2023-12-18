using HyperDimension.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperDimension.Infrastructure.Database.Configuration.Identity;

public class TotpConfiguration : IEntityTypeConfiguration<Totp>
{
    public void Configure(EntityTypeBuilder<Totp> builder)
    {
        builder
            .HasMany(x => x.RecoveryCodes);
    }
}
