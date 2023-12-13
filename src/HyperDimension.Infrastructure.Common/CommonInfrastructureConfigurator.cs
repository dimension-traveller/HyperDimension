using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Common.Enums;
using HyperDimension.Infrastructure.Common.Exceptions;
using HyperDimension.Infrastructure.Common.Options.Email;
using HyperDimension.Infrastructure.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Common;

public static class CommonInfrastructureConfigurator
{
    public static void AddCommonInfrastructure(this IServiceCollection services)
    {
        services.AddEmailServices();
    }

    private static void AddEmailServices(this IServiceCollection services)
    {
        var emailOptions = HyperDimensionConfiguration.Instance
            .GetOption<EmailOptions>();

        services.AddSingleton<IEmailService, EmailService>();
        var emailServicesBuilder = services
            .AddFluentEmail(emailOptions.From.ExpectNotNull(), emailOptions.FromName.ExpectNotNull())
            .AddRazorRenderer();

        switch (emailOptions.Sender)
        {
            case EmailSender.Smtp:
                if (emailOptions.Smtp is null)
                {
                    throw new EmailSenderNotSupportedException(EmailSender.Smtp.ToString(), "Smtp options are not configured.");
                }
                emailServicesBuilder.AddMailKitSender(emailOptions.Smtp);
                break;
            case EmailSender.MailGun:
                if (emailOptions.MailGun is null)
                {
                    throw new EmailSenderNotSupportedException(EmailSender.MailGun.ToString(), "MailGun options are not configured.");
                }
                emailServicesBuilder.AddMailGunSender(
                    emailOptions.MailGun.Domain, emailOptions.MailGun.ApiKey, emailOptions.MailGun.Region);
                break;
            case EmailSender.SendGrid:
                if (emailOptions.SendGrid is null)
                {
                    throw new EmailSenderNotSupportedException(EmailSender.SendGrid.ToString(), "SendGrid options are not configured.");
                }
                emailServicesBuilder.AddSendGridSender(
                    emailOptions.SendGrid.ApiKey, emailOptions.SendGrid.SandBoxMode);
                break;
            default:
                throw new EmailSenderNotSupportedException(emailOptions.Sender.ToString(), "Unknown email sender.");
        }
    }
}
