using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Search.Builder;
using HyperDimension.Infrastructure.Search.Enums;
using HyperDimension.Infrastructure.Search.Exceptions;
using HyperDimension.Infrastructure.Search.Options;
using HyperDimension.Infrastructure.Search.Services;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace HyperDimension.Infrastructure.Search;

public static class SearchConfigurator
{
    public static void AddHyperDimensionSearch(this IServiceCollection services)
    {
        var searchOptions = HyperDimensionConfiguration.Instance
            .GetOption<SearchOptions>();

        switch (searchOptions.Type)
        {
            case SearchProviderType.None:
                services.AddScoped<IHyperDimensionSearchService, DatabaseSearchProvider>();
                break;
            case SearchProviderType.PGroonga:
                services.AddScoped<IHyperDimensionSearchService, PGroongaSearchProvider>();
                services.AddSingleton<IDatabaseOptionsBuilder<NpgsqlDbContextOptionsBuilder>, PGroongaDatabaseOptionsBuilder>();
                break;
            case SearchProviderType.ElasticSearch:
                services.AddScoped<IHyperDimensionSearchService, ElasticSearchProvider>();
                break;
            case SearchProviderType.MeiliSearch:
                services.AddScoped<IHyperDimensionSearchService, MeiliSearchProvider>();
                break;
            case SearchProviderType.Algolia:
                services.AddScoped<IHyperDimensionSearchService, AlgoliaSearchProvider>();
                break;
            default:
                throw new SearchNotSupportedException(searchOptions.Type.ToString(), "Unknown search provider type.");
        }
    }
}
