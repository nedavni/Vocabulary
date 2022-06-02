using System;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Messaging.Callbacks;

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