using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Infrastructure.Database.Exceptions;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Database.Builder;

public class MySqlDatabaseBuilder : IDatabaseBuilder
{
    private readonly DatabaseOptions _databaseOptions;

    public MySqlDatabaseBuilder(DatabaseOptions databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public void Build(DbContextOptionsBuilder optionsBuilder)
    {
        throw new DatabaseNotSupportedException(_databaseOptions.Type.ToString(), "Waiting for Pomelo.EntityFrameworkCore.MySql 8.0.0");
    }
}
