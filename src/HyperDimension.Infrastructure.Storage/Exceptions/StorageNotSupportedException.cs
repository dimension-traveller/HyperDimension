namespace HyperDimension.Infrastructure.Storage.Exceptions;

public class StorageNotSupportedException(string type, string reason)
    : Exception($"Storage type {type} is not supported yet. Reason: {reason}");
