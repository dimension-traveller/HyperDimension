namespace HyperDimension.Infrastructure.Database.Exceptions;

public class DatabaseNotSupportedException(string type, string reason)
    : Exception($"Database type {type} is not supported yet. Reason: {reason}");
