﻿using HyperDimension.Common.Attributes;

namespace HyperDimension.Infrastructure.Cache.Options.Providers;

[OptionSection("Cache:Redis")]
public class RedisOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";

    public int Database { get; set; }

    public string KeyPrefix { get; set; } = "hd-";

    public bool Tracing { get; set; } = true;
}
