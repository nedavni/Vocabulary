using System;

namespace BotCore.Cache;

public interface IDataCache
{
    Guid Store(string value);

    bool TryGet(Guid key, out string value);
}