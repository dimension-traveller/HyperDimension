using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Database.Attributes;
using HyperDimension.Infrastructure.Database.Enums;

namespace HyperDimension.Infrastructure.Database.Extensions;

public static class ForDatabaseExtensions
{
    public static IEnumerable<Type> ForDatabase(this IEnumerable<Type> sources, DatabaseType databaseType)
    {
        return
            from s in sources
            let attr = s.GetAttribute<ForDatabaseAttribute>()
            where attr is null || attr.DatabaseType == databaseType
            select s;
    }
}
