using Amazon.S3;
using HyperDimension.Infrastructure.Storage.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HyperDimension.Infrastructure.Storage.HealthChecks;

public class S3HealthCheck : IHealthCheck
{
    private readonly AmazonS3Client _client;
    private readonly S3Options _s3Options;

    public S3HealthCheck(AmazonS3Client client, S3Options s3Options)
    {
        _client = client;
        _s3Options = s3Options;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var listBucketsResponse = await _client.ListBucketsAsync(cancellationToken);

        if (listBucketsResponse is null)
        {
            return new HealthCheckResult(context.Registration.FailureStatus);
        }

        if (listBucketsResponse.Buckets.Select(x => x.BucketName).Contains(_s3Options.BucketName))
        {
            return HealthCheckResult.Healthy();
        }

        return new HealthCheckResult(context.Registration.FailureStatus);
    }
}
