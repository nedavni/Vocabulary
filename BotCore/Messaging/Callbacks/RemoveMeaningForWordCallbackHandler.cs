using System.Text.Json;
using System.Threading.Tasks;
using BotCore.Cache;
using BotCore.Messaging.Dto;
using Telegram.Bot;

namespace BotCore.Messaging.Callbacks;

internal class RemoveMeaningForWordCallbackHandler : CallbackHandlerBase
{
    public RemoveMeaningForWordCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.RemoveMeaningForWord;

    protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        var wordMeaning = JsonSerializer.Deserialize<RemoveMeaningForWordCallback>(callback.Data);

        // List all words that contains meaning with deletecallback
        
        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            text: $"Meaning {wordMeaning.Meaning} removed for word {wordMeaning.Word}");
    }
}