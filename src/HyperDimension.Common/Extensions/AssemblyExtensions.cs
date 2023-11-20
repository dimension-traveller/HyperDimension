using System.Reflection;

namespace HyperDimension.Common.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> Scan(this IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(x => x.GetExportedTypes());
    }

    public static IEnumerable<Type> Scan(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
    {
        return assemblies.Scan()
            .Where(predicate);
    }

    public static IEnumerable<Type> Scan<T>(this IEnumerable<Assembly> assemblies)
    {
        return assemblies.Scan(
            x => typeof(T).IsAssignableFrom(x)
                 && x is { IsInterface: false, IsAbstract: false });
    }

    public static IEnumerable<Type> Scan(this Assembly assembly)
    {
        return assembly.GetExportedTypes();
    }

    public static IEnumerable<Type> Scan(this Assembly assembly, Func<Type, bool> predicate)
    {
        return assembly.Scan()
            .Where(predicate);
    }

    public static IEnumerable<Type> Scan<T>(this Assembly assembly)
    {
        return assembly.Scan(
            x => typeof(T).IsAssignableFrom(x)
                 && x is { IsInterface: false, IsAbstract: false });
    }
}
