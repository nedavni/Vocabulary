using System;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot.Types.ReplyMarkups;
using Vocabulary;

namespace BotCore.Messaging.Callbacks;

internal class RemoveTextWithWordCallbackHandler : CallbackHandlerBase
{
    private readonly IVocabularyRepository _repository;

    public RemoveTextWithWordCallbackHandler(IDataCache cache, IVocabularyRepository repository) : base(cache)
    {
        _repository = repository;
    }

    public override bool CanHandle(UserCallback callback)
        => callback.Kind == CallbackKind.RemoveTextThatContainsWord ||
           callback.Kind == CallbackKind.RemoveExactText;

    protected override async Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
    {
        switch (kind)
        {
            case CallbackKind.RemoveTextThatContainsWord:
                await FindTextThatContainsWords(callback, botInstruments);
                break;
            case CallbackKind.RemoveExactText:
                await RemoveExactText(callback, botInstruments);
                break;
            default: throw new NotImplementedException($"{nameof(CallbackKind)} - {kind}");

        }
    }

    private Task FindTextThatContainsWords(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;

        var textResults = _repository.Texts(callback.UserId.AsRepositoryId()).Where(t=> t.Contains(callback.Data)).ToList();

        if (textResults.Count == 0)
        {
            return botClient.SendMessage($"There is no text that contains: {callback.Data}", update.CallbackQuery);
        }

        var replies = textResults.Select(text =>
        {
            var replyKeyboard = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData(
                    "Remove this text",
                    Cache.StoreCallback(CallbackKind.RemoveExactText, callback.UserId, text).Serialize()));

            return botClient.SendMessage(text, update.CallbackQuery, replyKeyboard);
        });

        return Task.WhenAll(replies);
    }

    private Task RemoveExactText(CallbackData callback, BotInstruments botInstruments)
    {
        var(botClient, update, _) = botInstruments;
        return botClient.SendMessage($"Not implemented yet!", update.CallbackQuery);
    }

}