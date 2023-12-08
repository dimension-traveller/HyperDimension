using FluentEmail.Core;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Attributes;
using HyperDimension.Domain.Email;
using HyperDimension.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Infrastructure.Common.Services;

public class EmailService : IEmailService
{
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly IFluentEmail _fluentEmail;

    public EmailService(
        IStringLocalizerFactory stringLocalizerFactory,
        IFluentEmail fluentEmail)
    {
        _stringLocalizerFactory = stringLocalizerFactory;
        _fluentEmail = fluentEmail;
    }

    public async Task<Result<bool>> SendEmailAsync<T>(string to, string subjectKey, T model) where T : class, IEmailTemplate
    {
        var localizer = _stringLocalizerFactory.Create(typeof(T));
        var subject = localizer[subjectKey];

        var templateFile = typeof(T)
            .GetAttribute<EmailTemplateAttribute>()
            .ExpectNotNull()
            .TemplateName;

        var data = new EmailTemplate<T>
        {
            Localizer = localizer,
            Data = model
        };

        var response = await _fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded(
                templateFile,
                data,
                DomainAssemblyReference.Assembly)
            .SendAsync();

        if (response.Successful)
        {
            return true;
        }

        return string.Join(";", response.ErrorMessages);
    }
}
