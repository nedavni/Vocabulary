using System;
using BotCore.Messaging.Messages;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore;

internal static class UpdateExtensions
{
    public static BotMessage GetMessage(this Update update) =>
        update.Type switch
        {
            UpdateType.Message => BuildMessage(() => update.Message),
            UpdateType.EditedMessage => BuildMessage(() => update.EditedMessage),
            _ => throw new NotImplementedException($"Can't build BotMessage from: {update.Type}")
        };

    private static BotMessage BuildMessage(Func<Message> messageExtractor)
    {
        var message = messageExtractor();

        return new BotMessage(
            GetKind(message.Text.ToLowerInvariant()),
            message.Text,
            message.Chat.Id,
            message.From.Id,
            message.From.Username);

        static MessageKind GetKind(string text) =>
            text switch
            {
                "/start" => MessageKind.Start,
                "/menu" => MessageKind.Menu,
                "/traintranslation" => MessageKind.TrainTranslation,
                _ => MessageKind.Unknown
            };
    }
}