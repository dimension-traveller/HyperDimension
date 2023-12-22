using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Storage.Enums;
using HyperDimension.Infrastructure.Storage.Exceptions;
using HyperDimension.Infrastructure.Storage.HealthChecks;
using HyperDimension.Infrastructure.Storage.Options;
using HyperDimension.Infrastructure.Storage.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Storage;

public static class StorageConfigurator
{
    public static void AddHyperDimensionStorage(this IServiceCollection services)
    {
        var storageOptions = HyperDimensionConfiguration.Instance
            .GetOption<StorageOptions>();

        switch (storageOptions.Type)
        {
            case StorageType.FileSystem:
                services.AddSingleton<IHyperDimensionStorageProvider, FileSystemProvider>();
                break;
            case StorageType.S3:
                services.AddSingleton<IHyperDimensionStorageProvider, S3Provider>();
                services.AddHealthChecks().AddCheck<S3HealthCheck>("s3");
                break;
            default:
                throw new StorageNotSupportedException(storageOptions.Type.ToString(), "Unknown storage type.");
        }
    }
}
