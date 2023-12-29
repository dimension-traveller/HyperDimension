using HyperDimension.Domain.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperDimension.Infrastructure.Database.Configuration.Content;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.Slug);
        builder.HasIndex(x => x.CreatedAt);

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Articles);
    }
}
