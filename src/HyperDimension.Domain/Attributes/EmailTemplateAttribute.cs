namespace HyperDimension.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EmailTemplateAttribute : Attribute
{
    public string TemplateName { get; set; }

    public EmailTemplateAttribute(string templateName)
    {
        TemplateName = templateName;
    }
}
