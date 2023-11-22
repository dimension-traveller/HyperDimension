using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Storage.Enums;

namespace HyperDimension.Infrastructure.Storage.Options;

[OptionSection("Storage:S3")]
public class S3Options
{
    public S3Flavor Flavor { get; set; } = S3Flavor.Amazon;

    public string Endpoint { get; set; } = string.Empty;

    public string AccessKey { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public string BucketName { get; set; } = "hyper-dimension";

    public string Region { get; set; } = "ap-east-1";

    public bool ForcePathStyle { get; set; }
}
