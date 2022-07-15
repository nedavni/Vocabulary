using System;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;
using Vocabulary;

namespace BotCore.Messaging.Callbacks;

internal class RemoveWordCallbackHandler : CallbackHandlerBase
{
    private readonly IVocabularyRepository _repository;

    public RemoveWordCallbackHandler(IDataCache cache, IVocabularyRepository repository) : base(cache)
    {
        _repository = repository;
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.RemoveWord;

    protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;

        var wordToRemove = callback.Data.AsRepositoryString();

        try
        {
            var meanings = _repository.UserVocabulary(callback.UserId.AsRepositoryId())
                .Single(w => w.Word == wordToRemove)
                .Meanings;

            _repository.RemoveWord(callback.UserId.AsRepositoryId(), wordToRemove);

            return botClient.SendMessage($"Removed:\n<b>{wordToRemove} - {string.Join(',', meanings)}</b>", update.CallbackQuery);
        }
        catch (InvalidOperationException e) when (e.Message == "Sequence contains no matching element")
        {
            return botClient.SendMessage($"There is no saved word: <b>{wordToRemove}</b>", update.CallbackQuery);
        }
    }
}