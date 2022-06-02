using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BotCore.Messaging.Messages
{
    [Export(typeof(IMessageHandler<BotMessage>))]
    internal class StartMessageHandler : IMessageHandler<BotMessage>
    {
        public bool CanHandle(BotMessage message) => message.Kind == MessageKind.Start;

        public Task Handle(BotMessage message, BotInstruments instruments) =>
            instruments.BotClient.SendTextMessageAsync(
                message.ChatId,
                $"Hello {message.UserName}!\nYou can add pair of words using following formatting" +
                " <code>word <b>_</b> meaning</code> or add a plain text. Just start typing :)",
                ParseMode.Html,
                cancellationToken: instruments.CancellationToken);

    }
}
