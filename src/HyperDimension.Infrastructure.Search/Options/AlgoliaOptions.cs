namespace HyperDimension.Infrastructure.Search.Options;

public class AlgoliaOptions
{
    public string ApplicationId { get; set; } = "your-id";

    public string ApiKey { get; set; } = "your-key";

    public string IndexPrefix { get; set; } = "hd-";
}
