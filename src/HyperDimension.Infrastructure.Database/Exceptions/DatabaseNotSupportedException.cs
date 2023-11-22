using HyperDimension.Common.Exceptions;

namespace HyperDimension.Infrastructure.Database.Exceptions;

public class DatabaseNotSupportedException(string type, string reason)
    : UnsupportedException(type, reason, "Database");
