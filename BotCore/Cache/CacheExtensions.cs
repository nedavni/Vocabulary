using System;
using System.Text.Json;

namespace BotCore.Cache;

internal static class CacheExtensions
{
    public static UserCallback StoreCallback(this IDataCache cache, CallbackKind callbackKind, long userId, string data)
    {
        var cacheId = cache.Store(JsonSerializer.Serialize(new CallbackData(userId, data)));
        return new UserCallback(callbackKind, cacheId);
    }

    public static bool TryGetAsCallbackData(this IDataCache cache, Guid cacheId, out CallbackData data)
    {
        data = default;
        if (cache.TryGet(cacheId, out var rawData))
        {
            data = JsonSerializer.Deserialize<CallbackData>(rawData);
            return true;
        }

        return false;
    }
}