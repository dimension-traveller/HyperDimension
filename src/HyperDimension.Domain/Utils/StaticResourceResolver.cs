using System.Diagnostics.CodeAnalysis;
using HyperDimension.Common;
using HyperDimension.Domain.Resources;

namespace HyperDimension.Domain.Utils;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public static class StaticResourceResolver
{
    public static Result<Stream> GetResourceStreamAsync(string path)
    {
        var assembly = DomainAssemblyReference.Assembly;

        try
        {
            var resourceStream = assembly.GetManifestResourceStream(path);
            if (resourceStream is null)
            {
                return "Resource not found";
            }

            return resourceStream;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}
