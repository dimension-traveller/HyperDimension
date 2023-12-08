using HyperDimension.Common.Exceptions;

namespace HyperDimension.Infrastructure.Common.Exceptions;

public class EmailSenderNotSupportedException(string type, string reason)
    : UnsupportedException(type, reason, "Email Sender");
