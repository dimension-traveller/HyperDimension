namespace HyperDimension.Infrastructure.Search.Exceptions;

public class FieldNotSearchableException : Exception
{
    public FieldNotSearchableException(Type type)
        : base($"Filed {type.FullName} is not searchable.")
    {
    }
}
