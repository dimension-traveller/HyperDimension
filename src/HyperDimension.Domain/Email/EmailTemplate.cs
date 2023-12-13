using HyperDimension.Common.Options;
using HyperDimension.Domain.Abstract;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Domain.Email;

public class EmailTemplate<T> where T : class, IEmailTemplate
{
    public IStringLocalizer Localizer { get; set; } = null!;

    public ApplicationOptions ApplicationOptions { get; set; } = null!;

    public MetadataOptions MetadataOptions { get; set; } = null!;

    public T Data { get; set; } = null!;
}
