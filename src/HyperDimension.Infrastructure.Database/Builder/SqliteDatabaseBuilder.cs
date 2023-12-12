using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HyperDimension.Infrastructure.Database.Builder;

public class SqliteDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly IEnumerable<IDatabaseOptionsBuilder<SqliteDbContextOptionsBuilder>> _builders;

    public SqliteDatabaseBuilder(
        DatabaseOptions databaseOptions,
        IEnumerable<IDatabaseOptionsBuilder<SqliteDbContextOptionsBuilder>> builders)
    {
        _databaseOptions = databaseOptions;
        _builders = builders;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_databaseOptions.ConnectionString, options =>
        {
            foreach (var builder in _builders)
            {
                builder.Build(options);
            }
        });

        var sqliteDataSource = new SqliteConnectionStringBuilder(_databaseOptions.ConnectionString).DataSource;
        if (string.Equals(sqliteDataSource, ":memory:", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        sqliteDataSource.EnsureFileExist();
    }
}
