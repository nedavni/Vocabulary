using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Messaging.Callbacks
{
    internal class AddTextCallbackHandler : CallbackHandlerBase
    {
        public AddTextCallbackHandler(IDataCache cache) : base(cache)
        {
        }

        public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.AddText;

        protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
        {
            return botInstruments.BotClient.SendTextMessageAsync(
                botInstruments.Update.CallbackQuery.Message.Chat.Id,
                $"Added as text {callback.Data}",
                cancellationToken: botInstruments.CancellationToken);
        }
    }
}
