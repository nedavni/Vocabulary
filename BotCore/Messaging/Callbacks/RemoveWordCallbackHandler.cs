using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Messaging.Callbacks;

internal class RemoveWordCallbackHandler : CallbackHandlerBase
{
    public RemoveWordCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.RemoveWord;

    protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            $"Word {callback.Data} removed with all meanings");    }
}