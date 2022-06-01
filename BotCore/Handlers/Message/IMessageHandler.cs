using System.Threading.Tasks;

namespace BotCore.Handlers.Message
{
    public interface IMessageHandler
    {
        public int Order { get; }

        public bool CanHandle(string message);

        public Task Handle(BotMessage message, BotInstruments instruments);
    }
}
