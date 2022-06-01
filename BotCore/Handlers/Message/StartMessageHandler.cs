using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore.Handlers.Message
{
    [Export(typeof(IMessageHandler))]
    internal class StartMessageHandler : IMessageHandler
    {
        public int Order => 0;

        public bool CanHandle(string message) => message == "/start";

        public Task Handle(string message, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) =>
            botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                $"Hello {update.Message.From.Username}!\nYou can add pair of words using following formatting" +
                " <code>word <b>_</b> meaning</code> or add a plain text. Just start typing :)",
                ParseMode.Html,
                cancellationToken: cancellationToken);
        
    }
}
