namespace HyperDimension.Common.Extensions;

public static class ActionExtensions
{
    public static void ApplyActions<T>(this T obj, IEnumerable<Action<T>> actions)
    {
        foreach (var action in actions)
        {
            action.Invoke(obj);
        }
    }
}
