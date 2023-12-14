using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Search.Enums;

namespace HyperDimension.Infrastructure.Search.Options;

[OptionSection("Search")]
public class SearchOptions
{
    public SearchProviderType Type { get; set; } = SearchProviderType.Database;

    public ElasticsearchOptions? ElasticSearch { get; set; }

    public MeiliSearchOptions? MeiliSearch { get; set; }

    public AlgoliaOptions? Algolia { get; set; }
}
