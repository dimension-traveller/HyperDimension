using HyperDimension.Infrastructure.Search.Enums;

namespace HyperDimension.Infrastructure.Search.Options;

public class ElasticsearchOptions
{
    public string[] Nodes { get; set; } = ["http://localhost:9200"];

    public ElasticsearchAuthenticationMethod AuthenticationMethod { get; set; } = ElasticsearchAuthenticationMethod.Basic;

    public string? Username { get; set; } = "elastic";

    public string? Password { get; set; } = "changeme";

    public string? ApiKey { get; set; } = string.Empty;

    public string IndexPrefix { get; set; } = "hd-";

    public string? CertificateFingerprint { get; set; }
}
