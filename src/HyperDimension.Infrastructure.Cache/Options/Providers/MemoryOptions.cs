using HyperDimension.Common.Attributes;

namespace HyperDimension.Infrastructure.Cache.Options.Providers;

[OptionSection("Cache:Memory")]
public class MemoryOptions
{
    public int SizeLimit { get; set; } = 1024;
}
