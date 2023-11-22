using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Infrastructure.Storage.Enums;
using HyperDimension.Infrastructure.Storage.Exceptions;
using HyperDimension.Infrastructure.Storage.Options;
using HyperDimension.Infrastructure.Storage.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Storage;

public static class StorageConfigurator
{
    public static void AddHyperDimensionStorage(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IHyperDimensionStorageProvider), sp =>
        {
            var storageOptions = sp.GetRequiredService<StorageOptions>();

            switch (storageOptions.Type)
            {
                case StorageType.FileSystem:
                    var fileSystemOptions = sp.GetRequiredService<FileSystemOptions>();
                    return new FileSystemProvider(fileSystemOptions);
                case StorageType.S3:
                    var s3Options = sp.GetRequiredService<S3Options>();
                    return new S3Provider(s3Options);
                default:
                    throw new StorageNotSupportedException(storageOptions.Type.ToString(), "Unknown storage type.");
            }
        });
    }
}
