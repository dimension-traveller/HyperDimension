using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Database.Attributes;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Database.Builder;

[ForDatabase(DatabaseType.SQLite)]
public class SqliteDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;

    public SqliteDatabaseBuilder(
        DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_databaseOptions.ConnectionString, options =>
        {
            options.MigrationsAssembly("HyperDimension.Migrations.SQLite");
        });

        var sqliteDataSource = new SqliteConnectionStringBuilder(_databaseOptions.ConnectionString).DataSource;
        if (string.Equals(sqliteDataSource, ":memory:", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        sqliteDataSource.EnsureFileExist();
    }
}
