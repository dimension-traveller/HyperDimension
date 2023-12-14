using HyperDimension.Infrastructure.Database.Attributes;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Database.Builder;

[ForDatabase(DatabaseType.PostgreSQLWithPGroonga)]
public class NpgsqlWithPGroongaDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;

    public NpgsqlWithPGroongaDatabaseBuilder(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseOptions.ConnectionString, options =>
        {
            options.EnableRetryOnFailure(8);
            options.UsePGroonga();
            options.MigrationsAssembly("HyperDimension.Migrations.PostgreSQL");
        });
    }
}
