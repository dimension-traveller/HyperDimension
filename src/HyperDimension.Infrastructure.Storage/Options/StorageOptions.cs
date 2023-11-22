using HyperDimension.Common.Attributes;
using HyperDimension.Infrastructure.Storage.Enums;

namespace HyperDimension.Infrastructure.Storage.Options;

[OptionSection("Storage")]
public class StorageOptions
{
    public StorageType Type { get; set; } = StorageType.FileSystem;
}
