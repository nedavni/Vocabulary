using System.ComponentModel.Composition;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BotCore.Cache;
using BotCore.Handlers.Callback.Dto;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Callback;

[Export(typeof(ICallbackHandler))]
internal class RemoveMeaningCallbackHandler : CallbackHandlerBase
{
    public RemoveMeaningCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override CallbackKind CanHandleKind => CallbackKind.RemoveMeaning;

    protected override Task HandleInternal(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        // List all words that contains meaning with deletecallback

        var wordsWithMeaning = Enumerable
            .Range(1, 3)
            .Select(i => $"Word {i}")
            .Select(word =>

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        word,
                        Cache.StoreCallback(CallbackKind.RemoveMeaningForWord, callback.UserId,
                            JsonSerializer.Serialize(new RemoveMeaningForWordCallback(word, callback.Data))).Serialize())

                }
            );

        InlineKeyboardMarkup inlineKeyboard = new(wordsWithMeaning);

        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            text: "Choose the word for which remove meaning",
            replyMarkup: inlineKeyboard);
    }
}