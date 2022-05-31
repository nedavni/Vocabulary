using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Callback
{
    internal class MenuCallbackHandler : ICallbackHandler
    {
        public int Order { get; }

        public bool CanHandle(UserCallback callback) => callback.Command == BotCommands.Menu;

        public Task Handle(UserCallback callback, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                update.CallbackQuery.Message.Chat.Id,
                "/menu",
                replyMarkup: replyKeyboardMarkup);
        }
        
    }
}
