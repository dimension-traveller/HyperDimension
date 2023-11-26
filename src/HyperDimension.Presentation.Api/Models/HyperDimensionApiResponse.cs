namespace HyperDimension.Presentation.Api.Models;

public class HyperDimensionApiResponse
{
    public long TimeStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public string TraceId { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public object Data { get; set; } = new{ };
}
