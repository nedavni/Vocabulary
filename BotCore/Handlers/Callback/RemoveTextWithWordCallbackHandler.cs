using System;
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
internal class RemoveTextWithWordCallbackHandler : CallbackHandlerBase
{
    public RemoveTextWithWordCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override CallbackKind CanHandleKind => CallbackKind.RemoveTextThatContainsWord;

    protected override Task HandleInternal(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;

        var textGenerator = () => string.Join("\n", Enumerable.Range(1, 5).Select(i => Guid.NewGuid().ToString()));

        var replies = Enumerable
            .Range(1, 5)
            .Select(_ =>
            {
                var text = textGenerator();
                var replyKeyboard = new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData(
                        "Remove this text",
                        Cache.StoreCallback(CallbackKind.RemoveExactText, callback.UserId, text).Serialize()));

                return botClient.SendTextMessageAsync(
                    update.CallbackQuery.Message.Chat.Id,
                    text,
                    replyMarkup: replyKeyboard);
            });
            
        return Task.WhenAll(replies);
    }
}