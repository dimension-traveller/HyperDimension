using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Cache.Enums;

namespace HyperDimension.Infrastructure.Cache.Options;

[OptionSection("Cache")]
public class CacheOptions
{
    public CacheProvider Provider { get; set; } = CacheProvider.Memory;
}
