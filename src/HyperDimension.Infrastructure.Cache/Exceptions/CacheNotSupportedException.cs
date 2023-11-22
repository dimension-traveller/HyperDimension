using HyperDimension.Common.Exceptions;

namespace HyperDimension.Infrastructure.Cache.Exceptions;

public class CacheNotSupportedException(string type, string reason)
    : UnsupportedException(type, reason, "Cache");
