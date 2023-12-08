using System.Globalization;

namespace HyperDimension.Common.Extensions;

public static class DateTimeOffsetExtensions
{
    public static string GetUnixTimestamp(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToUnixTimeMilliseconds().ToString(NumberFormatInfo.InvariantInfo);
    }
}
