using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Search.Enums;
using HyperDimension.Infrastructure.Search.Exceptions;
using HyperDimension.Infrastructure.Search.Options;
using HyperDimension.Infrastructure.Search.Services;
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
                break;
            case SearchProviderType.MeiliSearch:
                throw new SearchNotSupportedException(SearchProviderType.MeiliSearch.ToString(), "Not implemented yet.");
            case SearchProviderType.Algolia:
                throw new SearchNotSupportedException(SearchProviderType.MeiliSearch.ToString(), "Not implemented yet.");
            default:
                throw new SearchNotSupportedException(searchOptions.Type.ToString(), "Unknown search provider type.");
        }
    }
}
