using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Database.Enums;

namespace HyperDimension.Infrastructure.Database.Options;

[OptionSection("Database")]
public class DatabaseOptions
{
    public DatabaseType Type { get; set; } = DatabaseType.SQLite;

    public string ConnectionString { get; set; } = "Data Source=data/HyperDimension.db;Version=3;";

    public bool Tracing { get; set; } = true;
}
