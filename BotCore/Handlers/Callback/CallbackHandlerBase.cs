using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Handlers.Callback;

internal abstract class CallbackHandlerBase : ICallbackHandler
{
    protected CallbackHandlerBase(IDataCache cache)
    {
        Cache = cache;
    }

    protected IDataCache Cache { get; }

    public abstract CallbackKind CanHandleKind { get; }

    public virtual bool CanHandle(UserCallback callback) => CanHandleKind == callback.Kind;

    public Task Handle(UserCallback callback, BotInstruments botInstruments)
    {
        var (botClient, update, cancellationToken) = botInstruments;
        if (Cache.TryGetAsCallbackData(callback.Id, out var data))
        {
            return HandleInternal(data, botInstruments);
        }
        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            "Cant process request, try to repeat operation!",
            cancellationToken: cancellationToken);
    }

    protected abstract Task HandleInternal(CallbackData callback, BotInstruments botInstruments);
}