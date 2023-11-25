namespace HyperDimension.Infrastructure.Identity.Exceptions;

public class DuplicatedAuthenticationSchemaException(string part, params string[] names)
    : Exception($"Found duplicated authentication schema {part}: " + string.Join(", ", names));
