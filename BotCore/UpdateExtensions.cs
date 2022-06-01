using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore;

internal static class UpdateExtensions
{
    public static BotMessage GetMessage(this Update update) => 
        update.Type switch
        {
            UpdateType.Message => new BotMessage(update.Message.Text, update.Message.Chat.Id, update.Message.From.Id, update.Message.From.Username),
            UpdateType.EditedMessage => new BotMessage(update.EditedMessage.Text, update.EditedMessage.Chat.Id, update.EditedMessage.From.Id, update.EditedMessage.From.Username),
            _ => throw new NotImplementedException($"Can't build BotMessage from: {update.Type}")
        };
}