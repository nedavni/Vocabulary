using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Callback
{
    [Export(typeof(ICallbackHandler))]
    internal class MenuCallbackHandler : ICallbackHandler
    {
        public bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.Menu;

        public Task Handle(UserCallback callback, BotInstruments botInstruments)
        {
            var (botClient, update, _) = botInstruments;
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
