namespace HyperDimension.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EmailTemplateAttribute : Attribute
{
    public string TemplateName { get; set; }

    public string SubjectLocalizerKey { get; set; }

    public EmailTemplateAttribute(string templateName, string subjectLocalizerKey)
    {
        TemplateName = templateName;
        SubjectLocalizerKey = subjectLocalizerKey;
    }
}
