using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Handlers.Callback
{
    public interface ICallbackHandler
    {
        public int Order { get; }

        public bool CanHandle(UserCallback callback);

        public Task Handle(UserCallback callback, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
