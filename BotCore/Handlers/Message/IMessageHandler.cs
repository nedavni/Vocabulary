using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore.Handlers.Message
{
    internal interface IMessageHandler
    {
        public int Order { get; }

        public bool CanHandle(string message);

        public Task Handle(string message, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
