using HyperDimension.Infrastructure.Database.Enums;

namespace HyperDimension.Infrastructure.Database.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ForDatabaseAttribute : Attribute
{
    public DatabaseType DatabaseType { get; }

    public ForDatabaseAttribute(DatabaseType databaseType)
    {
        DatabaseType = databaseType;
    }
}
