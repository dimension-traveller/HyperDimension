using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Search.Enums;

namespace HyperDimension.Infrastructure.Search.Options;

[OptionSection("Search")]
public class SearchOptions
{
    public SearchProviderType Type { get; set; } = SearchProviderType.None;

    public ElasticsearchOptions? ElasticSearch { get; set; }
}
