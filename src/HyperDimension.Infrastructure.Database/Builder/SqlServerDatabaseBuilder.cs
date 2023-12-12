using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace HyperDimension.Infrastructure.Database.Builder;

public class SqlServerDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly IEnumerable<IDatabaseOptionsBuilder<SqlServerDbContextOptionsBuilder>> _builders;

    public SqlServerDatabaseBuilder(
        DatabaseOptions databaseOptions,
        IEnumerable<IDatabaseOptionsBuilder<SqlServerDbContextOptionsBuilder>> builders)
    {
        _databaseOptions = databaseOptions;
        _builders = builders;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_databaseOptions.ConnectionString, options =>
        {
            options.EnableRetryOnFailure(8);
            foreach (var builder in _builders)
            {
                builder.Build(options);
            }
        });
    }
}
