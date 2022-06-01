using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Handlers.Callback
{
    [Export(typeof(ICallbackHandler))]
    internal class AddTextCallbackHandler : CallbackHandlerBase
    {
        public AddTextCallbackHandler(IDataCache cache) : base(cache)
        {
        }

        public override CallbackKind CanHandleKind => CallbackKind.AddText;

        protected override Task HandleInternal(CallbackData callback, BotInstruments botInstruments)
        {
            return botInstruments.BotClient.SendTextMessageAsync(
                botInstruments.Update.CallbackQuery.Message.Chat.Id,
                $"Added as text {callback.Data}",
                cancellationToken: botInstruments.CancellationToken);
        }
    }
}
