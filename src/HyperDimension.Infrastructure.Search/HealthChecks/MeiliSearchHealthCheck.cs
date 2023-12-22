using Meilisearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HyperDimension.Infrastructure.Search.HealthChecks;

public class MeiliSearchHealthCheck : IHealthCheck
{
    private readonly MeilisearchClient _client;

    public MeiliSearchHealthCheck(MeilisearchClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var meiliSearchHealth = await _client.HealthAsync(cancellationToken);

        if (meiliSearchHealth is null)
        {
            return new HealthCheckResult(context.Registration.FailureStatus);
        }

        return meiliSearchHealth.Status switch
        {
            "available" => HealthCheckResult.Healthy(),
            _ => new HealthCheckResult(context.Registration.FailureStatus)
        };
    }
}
