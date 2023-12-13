using FluentEmail.Core;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Common.Extensions;
using HyperDimension.Common.Options;
using HyperDimension.Common.Utilities;
using HyperDimension.Domain.Abstract;
using HyperDimension.Domain.Attributes;
using HyperDimension.Domain.Email;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Infrastructure.Common.Services;

public class EmailService : IEmailService
{
    private readonly ApplicationOptions _applicationOptions;
    private readonly MetadataOptions _metadataOptions;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly IFluentEmail _fluentEmail;

    public EmailService(
        ApplicationOptions applicationOptions,
        MetadataOptions metadataOptions,
        IStringLocalizerFactory stringLocalizerFactory,
        IFluentEmail fluentEmail)
    {
        _applicationOptions = applicationOptions;
        _metadataOptions = metadataOptions;
        _stringLocalizerFactory = stringLocalizerFactory;
        _fluentEmail = fluentEmail;
    }

    public async Task<Result<bool>> SendEmailAsync<T>(string to, T model) where T : class, IEmailTemplate
    {
        var emailTemplateAttribute = typeof(T)
            .GetAttribute<EmailTemplateAttribute>()
            .ExpectNotNull();

        var localizer = _stringLocalizerFactory.Create(typeof(T));
        var subject = localizer[emailTemplateAttribute.SubjectLocalizerKey];

        var data = new EmailTemplate<T>
        {
            Localizer = localizer,
            ApplicationOptions = _applicationOptions,
            MetadataOptions = _metadataOptions,
            Data = model
        };

        var response = await _fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded(
                emailTemplateAttribute.TemplateName,
                data,
                typeof(StaticResourceResolver).Assembly)
            .SendAsync();

        if (response.Successful)
        {
            return true;
        }

        return string.Join(";", response.ErrorMessages);
    }
}
