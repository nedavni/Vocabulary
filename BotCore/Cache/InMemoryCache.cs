using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;

namespace BotCore.Cache;

[Export(typeof(IDataCache))]
[PartCreationPolicy(CreationPolicy.Shared)]
internal sealed class InMemoryCache : IDataCache
{
    private readonly ConcurrentDictionary<Guid, (DateTime, string)> _cache;

    public InMemoryCache()
    {
        _cache = new ConcurrentDictionary<Guid, (DateTime, string)>();
    }

    public Guid Store(string value)
    {
        var key = Guid.NewGuid();
        _cache.TryAdd(key, (DateTime.UtcNow, value));
        return key;
    }

    public bool TryGet(Guid key, out string value)
    {
        value = default;
        if (_cache.TryGetValue(key, out var kv))
        {
            value = kv.Item2;
            return true;
        }
        return false;
    }
}