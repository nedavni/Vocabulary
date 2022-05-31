using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore.Handlers.Callback
{
    internal class AddKeyValueCallbackHandler : ICallbackHandler
    {
        public int Order => 100;

        public bool CanHandle(UserCallback callback) => callback.Command == BotCommands.AddKeyValue;

        public Task Handle(UserCallback callback, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var keyValue = callback.Payload.Split('_');
            if (keyValue.Length != 2)
            {
                return botClient.SendTextMessageAsync(
                    update.CallbackQuery.Message.Chat.Id,
                    "Invalid formatting! Pair should be formatted as <code>word <b>_</b> meaning</code>",
                    ParseMode.Html);
            }

            //TODO: list of all meanings
            return botClient.SendTextMessageAsync(
                update.CallbackQuery.Message.Chat.Id,
                $"For word {keyValue[0]} added meaning {keyValue[1]}",
                ParseMode.Html);
        }
    }
}
