using System.Threading.Tasks;

namespace BotCore.Messaging
{
    public interface IMessageHandler<in TMessage>
    {
        public bool CanHandle(TMessage message);

        public Task Handle(TMessage message, BotInstruments instruments);
    }
}
