using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Infrastructure.Storage.Options;
using HyperDimension.Infrastructure.Storage.Utilities;

namespace HyperDimension.Infrastructure.Storage.Providers;

public class FileSystemProvider : IHyperDimensionStorageProvider
{
    private readonly DirectoryInfo _directory;

    public FileSystemProvider(FileSystemOptions options)
    {
        _directory = new DirectoryInfo(options.RootPath);

        if (_directory.Exists is false)
        {
            _directory.Create();
        }
    }

    public async Task<string?> UploadFileAsync(string key, Stream fileStream)
    {
        var fk = key.GetFileKey();
        var filePath = Path.Combine(_directory.FullName, fk);

        await using var file = File.Create(filePath);
        fileStream.Seek(0, SeekOrigin.Begin);
        await fileStream.CopyToAsync(file);

        return fk;
    }

    public async Task<Stream?> DownloadFileAsync(string key)
    {
        var filePath = Path.Combine(_directory.FullName, key.GetFileKey());

        await using var fileStream = File.OpenRead(filePath);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);

        return memoryStream;
    }

    public Task DeleteFileAsync(string key)
    {
        var filePath = Path.Combine(_directory.FullName, key.GetFileKey());

        File.Delete(filePath);

        return Task.CompletedTask;
    }

    public Task<bool> FileExistsAsync(string key)
    {
        var filePath = Path.Combine(_directory.FullName, key.GetFileKey());

        return Task.FromResult(File.Exists(filePath));
    }

    public Task<IEnumerable<string>> ListFilesAsync(string path, bool recursive = false)
    {
        var directoryPath = Path.Combine(_directory.FullName, path.GetFilePath());
        var directory = new DirectoryInfo(directoryPath);

        if (directory.Exists is false)
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }

        var files = directory
            .GetFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        var fileNames = files
            .Select(file => file.FullName
                .Replace(_directory.FullName, string.Empty)
                .GetFileKey());

        return Task.FromResult(fileNames);
    }
}
