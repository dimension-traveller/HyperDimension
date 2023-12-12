using HyperDimension.Application.Common.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace HyperDimension.Infrastructure.Search.Builder;

public class PGroongaDatabaseOptionsBuilder : IDatabaseOptionsBuilder<NpgsqlDbContextOptionsBuilder>
{
    public void Build(NpgsqlDbContextOptionsBuilder builder)
    {
        builder.UsePGroonga();
    }
}
