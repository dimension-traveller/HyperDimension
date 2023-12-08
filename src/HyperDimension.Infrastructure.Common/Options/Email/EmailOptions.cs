using FluentEmail.MailKitSmtp;
using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Common.Enums;

namespace HyperDimension.Infrastructure.Common.Options.Email;

[OptionSection("Email")]
public class EmailOptions
{
    public bool Enabled { get; set; }

    public EmailSender Sender { get; set; } = EmailSender.Smtp;

    public string? From { get; set; } = "hyperdimension@example.com";

    public string? FromName { get; set; } = "Hyper Dimension";

    public SmtpClientOptions? Smtp { get; set; }

    public MailGunSenderOptions? MailGun { get; set; }

    public SendGridSenderOptions? SendGrid { get; set; }
}
