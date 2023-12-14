using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Infrastructure.Database.Attributes;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Database.Builder;

[ForDatabase(DatabaseType.MySQL)]
public class MySqlDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;

    public MySqlDatabaseBuilder(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        var forceDefault = Environment.GetEnvironmentVariable("HD_DEBUG_FORCE_DEFAULT_MYSQL") == "true";

        var mysqlServerVersion = forceDefault
            ? MySqlServerVersion.LatestSupportedServerVersion
            : ServerVersion.AutoDetect(_databaseOptions.ConnectionString);

        optionsBuilder.UseMySql(_databaseOptions.ConnectionString, mysqlServerVersion, options =>
        {
            options.EnableRetryOnFailure(8);
            options.MigrationsAssembly("HyperDimension.Migrations.MySQL");
        });
    }
}
