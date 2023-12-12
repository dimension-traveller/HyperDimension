using System.Globalization;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Common.Extensions;

public static class LocalizerExtensions
{
    public static string Format(this LocalizedString localizedString, params object[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, localizedString.Value, args);
    }
}
