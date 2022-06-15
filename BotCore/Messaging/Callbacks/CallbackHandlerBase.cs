using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Messaging.Callbacks;

[InheritedExport(typeof(IMessageHandler<UserCallback>))]
internal abstract class CallbackHandlerBase : IMessageHandler<UserCallback>
{
    protected CallbackHandlerBase(IDataCache cache)
    {
        Cache = cache;
    }

    protected IDataCache Cache { get; }

    public abstract bool CanHandle(UserCallback callback);

    public Task Handle(UserCallback callback, BotInstruments botInstruments)
    {
        var (botClient, update, cancellationToken) = botInstruments;
        if (Cache.TryGetAsCallbackData(callback.Id, out var data))
        {
            return HandleInternal(data, callback.Kind, botInstruments);
        }
        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            "Cant process request, try to repeat operation!",
            cancellationToken: cancellationToken);
    }

    protected abstract Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments);
}