using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot.Types.ReplyMarkups;
using Vocabulary;

namespace BotCore.Messaging.Callbacks;

internal class RemoveMeaningCallbackHandler : CallbackHandlerBase
{
    private readonly IVocabularyRepository _repository;

    public RemoveMeaningCallbackHandler(IDataCache cache, IVocabularyRepository repository) : base(cache)
    {
        _repository = repository;
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.RemoveMeaning || callback.Kind == CallbackKind.RemoveMeaningForWord;

    protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
        => kind switch
        {
            CallbackKind.RemoveMeaning => HandleRemoveMeaning(callback, botInstruments),
            CallbackKind.RemoveMeaningForWord => HandleRemoveMeaningForWord(callback, botInstruments),
            _ => throw new NotImplementedException($"Command {kind} not implemented!")
        };

    private Task HandleRemoveMeaning(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;

        var words = _repository.FindWordsWithMeaning(callback.UserId.AsRepositoryId(), callback.Data);

        var wordsWithMeaning =
            words
                .Select(word =>
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            word,
                            Cache.StoreCallback(CallbackKind.RemoveMeaningForWord, callback.UserId,
                                    JsonSerializer.Serialize(new RemoveMeaningForWordCallback(word, callback.Data)))
                                .Serialize())
                    }
                );

        InlineKeyboardMarkup inlineKeyboard = new(wordsWithMeaning);

        return botClient.SendMessage("Choose the word for which remove meaning", update.CallbackQuery, inlineKeyboard);
    }

    private Task HandleRemoveMeaningForWord(CallbackData callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        var (word, meaning) = JsonSerializer.Deserialize<RemoveMeaningForWordCallback>(callback.Data);

        var words = _repository.FindWordsWithMeaning(callback.UserId.AsRepositoryId(), meaning);

        if (words.Count == 0)
        {
            return botClient.SendMessage($"There is no saved words with meaning: {callback.Data}", update.CallbackQuery);
        }

        _repository.RemoveMeaning(callback.UserId.AsRepositoryId(), word, meaning);

        return botClient.SendMessage($"Meaning {meaning} removed for word {word}", update.CallbackQuery);
    }

    internal record struct RemoveMeaningForWordCallback(string Word, string Meaning);
}