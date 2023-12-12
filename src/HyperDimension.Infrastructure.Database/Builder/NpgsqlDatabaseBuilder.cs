using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace HyperDimension.Infrastructure.Database.Builder;

public class NpgsqlDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly IEnumerable<IDatabaseOptionsBuilder<NpgsqlDbContextOptionsBuilder>> _builders;

    public NpgsqlDatabaseBuilder(
        DatabaseOptions databaseOptions,
        IEnumerable<IDatabaseOptionsBuilder<NpgsqlDbContextOptionsBuilder>> builders)
    {
        _databaseOptions = databaseOptions;
        _builders = builders;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseOptions.ConnectionString, options =>
        {
            foreach (var builder in _builders)
            {
                builder.Build(options);
            }
        });
    }
}
