using HyperDimension.Common.Attributes;

namespace HyperDimension.Infrastructure.Storage.Options;

[OptionSection("Storage:FileSystem")]
public class FileSystemOptions
{
    public string RootPath { get; set; } = string.Empty;
}
