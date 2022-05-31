using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Message
{
    internal class MenuMessageHandler : IMessageHandler
    {
        public int Order => 100;

        public bool CanHandle(string message) => message == "/menu";

        public Task Handle(string message, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                {
                    new KeyboardButton[] {BotCommands.Train, BotCommands.Repeat }
                })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            return botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                "/menu",
                replyMarkup: replyKeyboardMarkup);
        }
        
    }
}
