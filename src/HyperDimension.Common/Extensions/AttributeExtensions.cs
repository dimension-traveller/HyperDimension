namespace HyperDimension.Common.Extensions;

public static class AttributeExtensions
{
    public static bool HasAttribute<T>(this Type type) where T : class
    {
        return type.HasAttribute(typeof(T));
    }

    public static bool HasAttribute(this Type type, Type attributeType)
    {
        return type.GetCustomAttributes(attributeType, false).Length != 0;
    }

    public static T? GetAttribute<T>(this Type type) where T : class
    {
        return type.GetAttribute(typeof(T)) as T;
    }

    public static object? GetAttribute(this Type type, Type attributeType)
    {
        return type.GetCustomAttributes(attributeType, false).FirstOrDefault();
    }
}
