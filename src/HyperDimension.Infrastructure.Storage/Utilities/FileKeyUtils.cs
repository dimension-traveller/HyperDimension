namespace HyperDimension.Infrastructure.Storage.Utilities;

public static class FileKeyUtils
{
    public static string GetFileKey(this string key)
    {
        return key.Replace('\\', '/').Trim().TrimStart('/');
    }

    public static string GetFilePath(this string path)
    {
        var p = path.Replace('\\', '/').Trim().Trim('/');
        return string.IsNullOrEmpty(p) ? string.Empty : $"{p}/";
    }
}
