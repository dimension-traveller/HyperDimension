namespace HyperDimension.Common.Exceptions;

public class UnsupportedException(string type, string reason, string module)
    : Exception($"Unsupported {module} type {type}. Reason: {reason}");
