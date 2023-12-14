using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Infrastructure.Database.Attributes;
using HyperDimension.Infrastructure.Database.Enums;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Database.Builder;

[ForDatabase(DatabaseType.SQLServer)]
public class SqlServerDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;

    public SqlServerDatabaseBuilder(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_databaseOptions.ConnectionString, options =>
        {
            options.EnableRetryOnFailure(8);
            options.MigrationsAssembly("HyperDimension.Migrations.SQLServer");
        });
    }
}
