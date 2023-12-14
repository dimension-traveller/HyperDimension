using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Database.Attributes;
using HyperDimension.Infrastructure.Database.Enums;

namespace HyperDimension.Infrastructure.Database.Extensions;

public static class ForDatabaseExtensions
{
    public static IEnumerable<T> ForDatabase<T>(this IEnumerable<T> sources, DatabaseType databaseType)
    {
        return
            from s in sources
            let attr = s.GetType().GetAttribute<ForDatabaseAttribute>()
            where attr is null || attr.DatabaseType == databaseType
            select s;
    }
}
