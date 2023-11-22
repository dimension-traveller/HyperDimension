using Amazon.S3;
using HyperDimension.Infrastructure.Storage.Enums;
using HyperDimension.Infrastructure.Storage.Exceptions;
using HyperDimension.Infrastructure.Storage.Options;

namespace HyperDimension.Infrastructure.Storage.Utilities;

public static class AmazonS3ClientBuilder
{
    public static AmazonS3Client Build(this S3Options options)
    {
        return options.Flavor switch
        {
            S3Flavor.Amazon => options.BuildAmazon(),
            S3Flavor.Generic => options.BuildGeneric(),
            _ => throw new StorageNotSupportedException($"S3-{options.Flavor}", "Unknown S3 flavor.")
        };
    }

    private static AmazonS3Client BuildAmazon(this S3Options options)
    {
        var s3Config = new AmazonS3Config
        {
            AuthenticationRegion = options.Region,
            ForcePathStyle = options.ForcePathStyle,
        };

        return new AmazonS3Client(options.AccessKey, options.SecretKey, s3Config);
    }

    private static AmazonS3Client BuildGeneric(this S3Options options)
    {
        var s3Config = new AmazonS3Config
        {
            AuthenticationRegion = options.Region,
            ServiceURL = options.Endpoint,
            ForcePathStyle = options.ForcePathStyle,
        };

        return new AmazonS3Client(options.AccessKey, options.SecretKey, s3Config);
    }
}
