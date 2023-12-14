namespace HyperDimension.Infrastructure.Search.Options;

public class MeiliSearchOptions
{
    public string Url { get; set; } = "http://localhost:7700";

    public string ApiKey { get; set; } = "masterKey";

    public string IndexPrefix { get; set; } = "hd-";
}
