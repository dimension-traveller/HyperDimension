namespace HyperDimension.Common.Extensions;

public static class FileSystemExtensions
{
    public static bool EnsureDirectoryExist(this DirectoryInfo directoryInfo)
    {
        if (directoryInfo.Exists)
        {
            return true;
        }

        try
        {
            directoryInfo.Create();
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }

    public static bool EnsureDirectoryExist(this string directoryPath)
    {
        var directoryInfo = new DirectoryInfo(directoryPath);

        if (directoryInfo.Exists)
        {
            return true;
        }

        try
        {
            directoryInfo.Create();
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }

    public static bool EnsureFileExist(this FileInfo fileInfo)
    {
        if (fileInfo.Exists)
        {
            return true;
        }

        var parentDirectory = fileInfo.Directory;
        if (parentDirectory is null)
        {
            return false;
        }

        var directoryExist = parentDirectory.EnsureDirectoryExist();
        if (directoryExist is false)
        {
            return false;
        }

        try
        {
            var stream = fileInfo.Create();
            stream.Close();
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }

    public static bool EnsureFileExist(this string filePath)
    {
        var fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
        {
            return true;
        }

        var parentDirectory = fileInfo.Directory;
        if (parentDirectory is null)
        {
            return false;
        }

        var directoryExist = parentDirectory.EnsureDirectoryExist();
        if (directoryExist is false)
        {
            return false;
        }

        try
        {
            var stream = fileInfo.Create();
            stream.Close();
        }
        catch (IOException)
        {
            return false;
        }

        return true;
    }
}
