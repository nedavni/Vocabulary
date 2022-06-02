using System.ComponentModel.Composition;
using System.Text.Json;
using System.Threading.Tasks;
using BotCore.Cache;
using BotCore.Handlers.Callback.Dto;
using Telegram.Bot;

namespace BotCore.Handlers.Callback;

[Export(typeof(ICallbackHandler))]
internal class RemoveMeaningForWordCallbackHandler : CallbackHandlerBase
{
    public RemoveMeaningForWordCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override CallbackKind CanHandleKind => CallbackKind.RemoveMeaningForWord;

    protected override Task HandleInternal(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        var wordMeaning = JsonSerializer.Deserialize<RemoveMeaningForWordCallback>(callback.Data);

        // List all words that contains meaning with deletecallback
        
        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            text: $"Meaning {wordMeaning.Meaning} removed for word {wordMeaning.Word}");
    }
}