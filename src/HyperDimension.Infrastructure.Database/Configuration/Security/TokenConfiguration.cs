using HyperDimension.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperDimension.Infrastructure.Database.Configuration.Security;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder
            .HasIndex(x => x.Value)
            .IsUnique();
    }
}
