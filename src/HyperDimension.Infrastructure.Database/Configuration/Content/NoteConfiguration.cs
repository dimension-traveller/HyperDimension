using HyperDimension.Domain.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HyperDimension.Infrastructure.Database.Configuration.Content;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.Slug);
        builder.HasIndex(x => x.CreatedAt);
    }
}
