using FluentEmail.Mailgun;

namespace HyperDimension.Infrastructure.Common.Options.Email;

public class MailGunSenderOptions
{
    public string Domain { get; set; } = "example.com";

    public string ApiKey { get; set; } = string.Empty;

    public MailGunRegion Region { get; set; } = MailGunRegion.USA;
}
