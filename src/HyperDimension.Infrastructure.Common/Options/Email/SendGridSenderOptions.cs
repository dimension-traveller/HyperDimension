namespace HyperDimension.Infrastructure.Common.Options.Email;

public class SendGridSenderOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public bool SandBoxMode { get; set; }
}
