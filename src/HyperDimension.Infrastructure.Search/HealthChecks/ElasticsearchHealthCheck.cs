using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthStatus = Elastic.Clients.Elasticsearch.HealthStatus;

namespace HyperDimension.Infrastructure.Search.HealthChecks;

public class ElasticsearchHealthCheck : IHealthCheck
{
    private readonly ElasticsearchClient _client;

    public ElasticsearchHealthCheck(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var healthResponse = await _client.Cluster.HealthAsync(cancellationToken);

        if (healthResponse.TimedOut)
        {
            return new HealthCheckResult(context.Registration.FailureStatus);
        }

        return healthResponse.Status switch
        {
            HealthStatus.Green => HealthCheckResult.Healthy(),
            HealthStatus.Yellow => HealthCheckResult.Degraded(),
            HealthStatus.Red => HealthCheckResult.Unhealthy(),
            _ => new HealthCheckResult(context.Registration.FailureStatus)
        };
    }
}
