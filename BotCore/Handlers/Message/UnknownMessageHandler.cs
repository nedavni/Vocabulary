using System.ComponentModel.Composition;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Message
{
    [Export(typeof(IMessageHandler))]
    internal class UnknownMessageHandler : IMessageHandler
    {
        public int Order => int.MaxValue;

        public bool CanHandle(string message) => true;

        public Task Handle(string message, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                new[]
                {
                    new[]{
                    InlineKeyboardButton.WithCallbackData(
                        "Add as text",
                        JsonSerializer.Serialize(new UserCallback(BotCommands.AddText, update.Message.From.Id, update.Message.Text))),
                    InlineKeyboardButton.WithCallbackData(
                        "Add as word - meaning",
                        JsonSerializer.Serialize(new UserCallback(BotCommands.AddKeyValue, update.Message.From.Id, update.Message.Text)))
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(
                            BotCommands.Menu,
                            JsonSerializer.Serialize(new UserCallback(BotCommands.Menu, update.Message.From.Id, update.Message.Text)))
                    }
                });

            return botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                update.Message.Text!,
                replyMarkup: inlineKeyboard);
        }
    }
}
