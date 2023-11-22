using HyperDimension.Common.Exceptions;

namespace HyperDimension.Infrastructure.Storage.Exceptions;

public class StorageNotSupportedException(string type, string reason)
    : UnsupportedException(type, reason, "Storage");
