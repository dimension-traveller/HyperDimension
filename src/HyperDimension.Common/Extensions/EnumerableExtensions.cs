namespace HyperDimension.Common.Extensions;

public static class EnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> source, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        return (await source).ToList();
    }
}
