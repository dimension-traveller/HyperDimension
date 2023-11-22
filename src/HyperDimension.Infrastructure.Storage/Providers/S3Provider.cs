using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Infrastructure.Storage.Options;
using HyperDimension.Infrastructure.Storage.Utilities;

namespace HyperDimension.Infrastructure.Storage.Providers;

public class S3Provider(S3Options options) : IHyperDimensionStorageProvider, IDisposable
{
    private readonly AmazonS3Client _client = options.Build();

    public async Task<string?> UploadFileAsync(string key, Stream fileStream)
    {
        var fk = key.GetFileKey();
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);

        var req = new PutObjectRequest
        {
            BucketName = options.BucketName,
            Key = fk,
            InputStream = memoryStream,
            AutoCloseStream = true,
            AutoResetStreamPosition = true,
        };

        var result = await _client.PutObjectAsync(req);
        return result is not null ? fk : null;
    }

    public async Task<Stream?> DownloadFileAsync(string key)
    {
        var memoryStream = new MemoryStream();

        var req = new GetObjectRequest
        {
            BucketName = options.BucketName,
            Key = key.GetFileKey()
        };

        var result = await _client.GetObjectAsync(req);
        if (result is null)
        {
            return null;
        }

        await result.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public Task DeleteFileAsync(string key)
    {
        var req = new DeleteObjectRequest
        {
            BucketName = options.BucketName,
            Key = key.GetFileKey()
        };

        return _client.DeleteObjectAsync(req);
    }

    public async Task<bool> FileExistsAsync(string key)
    {
        var req = new GetObjectAttributesRequest
        {
            BucketName = options.BucketName,
            Key = key.GetFileKey()
        };

        var result = await _client.GetObjectAttributesAsync(req);
        return result?.HttpStatusCode is HttpStatusCode.OK;
    }

    public async Task<IEnumerable<string>> ListFilesAsync(string path, bool recursive = false)
    {
        var req = new ListObjectsV2Request
        {
            BucketName = options.BucketName,
            Prefix = path.GetFilePath(),
            Delimiter = recursive ? null : "/"
        };

        var result = await _client.ListObjectsV2Async(req);
        return result.S3Objects.Select(x => x.Key);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _client.Dispose();
    }
}
