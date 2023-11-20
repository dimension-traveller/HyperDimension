using System.Collections.Immutable;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace HyperDimension.Common;

public static class Constants
{
    public static string RuntimeEnvironment =>
        Environment.GetEnvironmentVariable("HD_RUNTIME_ENVIRONMENT")?.ToLowerInvariant() ??
        (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLowerInvariant() ?? "production");

    public static bool IsDevelopment => RuntimeEnvironment == "development";

    public static bool IsProduction => RuntimeEnvironment == "production";

    public static ImmutableArray<Assembly> ProjectAssemblies => DependencyContext.Default!.GetDefaultAssemblyNames()
        .Where(x => x.FullName.StartsWith("HyperDimension", StringComparison.InvariantCultureIgnoreCase))
        .Select(Assembly.Load)
        .ToImmutableArray();
}
