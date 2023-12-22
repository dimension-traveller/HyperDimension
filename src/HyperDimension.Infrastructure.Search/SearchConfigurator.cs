using Algolia.Search.Clients;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Search.Enums;
using HyperDimension.Infrastructure.Search.Exceptions;
using HyperDimension.Infrastructure.Search.HealthChecks;
using HyperDimension.Infrastructure.Search.Options;
using HyperDimension.Infrastructure.Search.Services;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Search;

public static class SearchConfigurator
{
    public static void AddHyperDimensionSearch(this IServiceCollection services)
    {
        var searchOptions = HyperDimensionConfiguration.Instance
            .GetOption<SearchOptions>();

        switch (searchOptions.Type)
        {
            case SearchProviderType.Database:
                services.AddScoped<IHyperDimensionSearchService, DatabaseSearchProvider>();
                break;
            case SearchProviderType.ElasticSearch:
                services.AddScoped<IHyperDimensionSearchService, ElasticSearchProvider>();
                services.AddSingleton<IElasticsearchClientSettings, ElasticsearchClientSettings>(_ =>
                {
                    var esOptions = searchOptions.ElasticSearch
                                    ?? throw new SearchNotSupportedException(SearchProviderType.ElasticSearch.ToString(), "Elasticsearch options is null.");

                    var staticNodePool = new StaticNodePool(esOptions.Nodes.Select(x => new Uri(x)));
                    var esClientSettings = new ElasticsearchClientSettings(staticNodePool);
                    switch (esOptions.AuthenticationMethod)
                    {
                        case ElasticsearchAuthenticationMethod.Basic:
                            esClientSettings.Authentication(
                                new BasicAuthentication(esOptions.Username!, esOptions.Password!));
                            break;
                        case ElasticsearchAuthenticationMethod.ApiKey:
                            esClientSettings.Authentication(
                                new ApiKey(esOptions.ApiKey!));
                            break;
                        case ElasticsearchAuthenticationMethod.None:
                        default:
                            break;
                    }

                    if (string.IsNullOrEmpty(esOptions.CertificateFingerprint) is false)
                    {
                        esClientSettings.CertificateFingerprint(esOptions.CertificateFingerprint);
                    }

                    return esClientSettings;
                });
                services.AddSingleton<ElasticsearchClient>(sp =>
                {
                    var esClientSettings = sp.GetRequiredService<IElasticsearchClientSettings>();
                    return new ElasticsearchClient(esClientSettings);
                });
                services.AddHealthChecks().AddCheck<ElasticsearchHealthCheck>("elasticsearch");
                break;
            case SearchProviderType.MeiliSearch:
                services.AddScoped<IHyperDimensionSearchService, MeiliSearchProvider>();
                services.AddSingleton<MeilisearchClient>(_ =>
                {
                    var meiliOptions = searchOptions.MeiliSearch
                                       ?? throw new SearchNotSupportedException(SearchProviderType.MeiliSearch.ToString(), "MeiliSearch options is null.");
                    return new MeilisearchClient(meiliOptions.Url, meiliOptions.ApiKey);
                });
                services.AddHealthChecks().AddCheck<MeiliSearchHealthCheck>("meili-search");
                break;
            case SearchProviderType.Algolia:
                services.AddScoped<IHyperDimensionSearchService, AlgoliaSearchProvider>();
                services.AddSingleton<ISearchClient, SearchClient>(sp =>
                {
                    var algoliaOptions = searchOptions.Algolia
                                         ?? throw new SearchNotSupportedException(SearchProviderType.Algolia.ToString(), "Algolia options is null.");

                    return new SearchClient(algoliaOptions.ApplicationId, algoliaOptions.ApiKey);
                });
                break;
            default:
                throw new SearchNotSupportedException(searchOptions.Type.ToString(), "Unknown search provider type.");
        }
    }
}
