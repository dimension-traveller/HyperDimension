using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

internal sealed class PGroongaExtensionInfo : DbContextOptionsExtensionInfo
{
    public override bool IsDatabaseProvider => false;
    public override string LogFragment => "using PGroonga ";

    public PGroongaExtensionInfo(IDbContextOptionsExtension extension)
        : base(extension) { }

    public override int GetServiceProviderHashCode()
    {
        return 0;
    }

    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
    {
        return true;
    }

    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
    {
        debugInfo[$"Npgsql: {nameof(PGroongaConfigurator.UsePGroonga)}"] = "1";
    }
}
