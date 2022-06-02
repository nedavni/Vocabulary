using System;
using System.Text.Json;

namespace BotCore.Messaging.Callbacks;

public record struct UserCallback(CallbackKind Kind, Guid Id)
{
    public string Serialize() => JsonSerializer.Serialize(this);
}

public record struct CallbackData(long UserId, string Data);