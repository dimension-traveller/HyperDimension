using HyperDimension.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperDimension.Infrastructure.Database.Configuration.Identity;

public class ApiTokenConfiguration : IEntityTypeConfiguration<ApiToken>
{
    public void Configure(EntityTypeBuilder<ApiToken> builder)
    {
        builder
            .HasIndex(x => x.Token)
            .IsUnique();
    }
}
