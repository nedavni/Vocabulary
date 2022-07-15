using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BotCore.Cache;
using BotCore.Messaging.Callbacks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Vocabulary;

namespace BotCore.Messaging.Messages;

[Export(typeof(IMessageHandler<BotMessage>))]
internal class TrainTranslationMessageHandler : CallbackHandlerBase, IMessageHandler<BotMessage>
{
    private readonly IDataCache _cache;
    private readonly IVocabularyRepository _repository;
    private readonly Random _random;

    public TrainTranslationMessageHandler(IDataCache cache, IVocabularyRepository repository) : base(cache)
    {
        _cache = cache;
        _repository = repository;
        _random = new Random();
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.CorrectMeaningChosen ||
                                                             callback.Kind == CallbackKind.WrongMeaningChosen;

    public bool CanHandle(BotMessage message) => message.Kind == MessageKind.TrainTranslation;

    public Task Handle(BotMessage message, BotInstruments instruments)
    {
        var userVocabulary = _repository.UserVocabulary(message.UserId.AsRepositoryId());

        if (userVocabulary.Count == 1)
        {
            return instruments.BotClient.SendMessage($"There's nothing to train, you have only {userVocabulary.Count}!", message.ChatId);
        }

        var (wordToTrain, correctMeaning, wrongMeanings) = PrepareTrainWord(userVocabulary);

        return MessageWithTranslationOptions(wordToTrain, correctMeaning, wrongMeanings, message.UserId, message.ChatId, instruments);
    }

    protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
        => kind switch
        {
            CallbackKind.CorrectMeaningChosen => HandleCorrectTranslation(callback, botInstruments),
            CallbackKind.WrongMeaningChosen => HandleWrongTranslation(callback, botInstruments),
            _ => throw new NotImplementedException($"Command {kind} not implemented!")
        };

    private async Task HandleWrongTranslation(CallbackData callback, BotInstruments instruments)
    {
        var (botClient, update, _) = instruments;

        await instruments.BotClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            "Wrong! Try one more time!");

        var (word, correctMeaning, wrongMeanings) = JsonSerializer.Deserialize<WrongTranslation>(callback.Data);
        await MessageWithTranslationOptions(word, correctMeaning, wrongMeanings, callback.UserId, update.CallbackQuery.Message.Chat.Id, instruments);
    }

    private async Task HandleCorrectTranslation(CallbackData callback, BotInstruments instruments)
    {
        // 1) print all meanings for word
        var trainedWord = JsonSerializer.Deserialize<CorrectTranslation>(callback.Data).Word.AsRepositoryString();
        // 2) go to next word

        var (botClient, update, _) = instruments;

        var meanings = _repository
            .UserVocabulary(callback.UserId.AsRepositoryId())
            .Single(w => w.Word == trainedWord)
            .Meanings;

        await instruments.BotClient.SendMessage($"CORRECT!\n{trainedWord} - {string.Join(",", meanings)}\nGO NEXT!", update.CallbackQuery);

        var userVocabulary = _repository.UserVocabulary(callback.UserId.AsRepositoryId());

        if (userVocabulary.Count == 1)
        {
            await instruments.BotClient.SendMessage($"There's nothing to train, you have only {userVocabulary.Count}!", update.CallbackQuery);
            return;
        }
        var (wordToTrain, correctMeaning, wrongMeanings) = PrepareTrainWord(userVocabulary);
        await MessageWithTranslationOptions(wordToTrain, correctMeaning, wrongMeanings, callback.UserId, update.CallbackQuery.Message.Chat.Id, instruments);
    }

    private Task MessageWithTranslationOptions(string word, string correctMeaning, IList<string> wrongMeanings, long userId, long chatId, BotInstruments instruments)
    {
        var correctCallback = InlineKeyboardButton.WithCallbackData(
            correctMeaning,
            _cache.StoreCallback(CallbackKind.CorrectMeaningChosen, userId, JsonSerializer.Serialize(new CorrectTranslation(word))).Serialize());
        var wrongCallbacks = wrongMeanings.Select(translation =>
       {
           return new[]
           {
                InlineKeyboardButton.WithCallbackData(
                    translation,
                    _cache.StoreCallback(CallbackKind.WrongMeaningChosen, userId, JsonSerializer.Serialize(new WrongTranslation(word, correctMeaning, wrongMeanings))).Serialize())
           };
       });

        var reply = wrongCallbacks.ToList();
        reply.Add(new[] { correctCallback });
        reply.Shuffle();

        InlineKeyboardMarkup inlineKeyboard = new(reply);

        return instruments.BotClient.SendMessage(word, chatId, inlineKeyboard);
    }

    private (string WordToTrain, string CorrectMeaning, IList<string> WrongMeanings) PrepareTrainWord(IReadOnlyList<WordWithMeanings> userVocabulary)
    {
        const int meaningCount = 3;

        var maxRandomExclusiveValue = userVocabulary.Count;
        var correctWordIndex = _random.Next(maxRandomExclusiveValue);
        var (wordToTrain, correctMeanings) = userVocabulary[correctWordIndex];

        var incorrectAnswers = userVocabulary.Count <= meaningCount
            ? userVocabulary.Where((_, i) => i != correctWordIndex)
            : userVocabulary.Where((_, i) => _random.Next(maxRandomExclusiveValue) != correctWordIndex);

        var correctMeaning = correctMeanings[_random.Next(correctMeanings.Count - 1)];
        var wrongMeanings = incorrectAnswers.Select(a => a.Meanings[_random.Next(a.Meanings.Count - 1)]).ToList();

        return new(wordToTrain, correctMeaning, wrongMeanings);
    }

    private record struct WrongTranslation(string Word, string CorrectMeaning, IList<string> WrongMeanings);

    private record struct CorrectTranslation(string Word);
}