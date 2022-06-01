using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Handlers.Callback;

[Export(typeof(ICallbackHandler))]
internal class RemoveWordCallbackHandler : CallbackHandlerBase
{
    public RemoveWordCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override CallbackKind CanHandleKind => CallbackKind.RemoveWord;

    protected override Task HandleInternal(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            $"Word {callback.Data} removed with all meanings");    }
}