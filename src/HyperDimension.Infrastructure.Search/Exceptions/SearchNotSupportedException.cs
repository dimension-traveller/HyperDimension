using HyperDimension.Common.Exceptions;

namespace HyperDimension.Infrastructure.Search.Exceptions;

public class SearchNotSupportedException(string type, string reason)
    : UnsupportedException(type, reason, "Search");
