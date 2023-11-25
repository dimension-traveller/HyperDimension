using HyperDimension.Common.Exceptions;

namespace HyperDimension.Infrastructure.Identity.Exceptions;

public class AuthenticationNotSupportedException(string type, string reason)
    : UnsupportedException(type, reason, "Authentication");
