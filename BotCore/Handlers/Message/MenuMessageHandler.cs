using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Message
{
    [Export(typeof(IMessageHandler))]
    internal class MenuMessageHandler : IMessageHandler
    {
        public int Order => 100;

        public bool CanHandle(string message) => message == "/menu";

        public Task Handle(BotMessage message, BotInstruments instruments)
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

            return instruments.BotClient.SendTextMessageAsync(
                message.ChatId,
                "/menu",
                replyMarkup: replyKeyboardMarkup);
        }
    }
}
