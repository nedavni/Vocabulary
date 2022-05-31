using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Handlers.Callback
{
    internal class AddTextCallbackHandler : ICallbackHandler
    {
        public int Order { get; }

        public bool CanHandle(UserCallback callback) => callback.Command == BotCommands.AddText;

        public Task Handle(UserCallback callback, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
           return botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"Added as text {callback.Payload}");
        }
    }
}
