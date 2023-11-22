namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionStorageProvider
{
    public Task<string?> UploadFileAsync(string key, Stream fileStream);
    public Task<Stream?> DownloadFileAsync(string key);
    public Task DeleteFileAsync(string key);
    public Task<bool> FileExistsAsync(string key);
    public Task<IEnumerable<string>> ListFilesAsync(string path, bool recursive = false);
}
