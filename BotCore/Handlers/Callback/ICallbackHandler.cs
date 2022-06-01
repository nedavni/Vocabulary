using System.Threading.Tasks;

namespace BotCore.Handlers.Callback
{
    public interface ICallbackHandler
    {
        public bool CanHandle(UserCallback callback);

        public Task Handle(UserCallback callback, BotInstruments botInstruments);
    }
}
