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

        var meanings = _repository.FindMeanings(callback.UserId.AsRepositoryId(), callback.Data);

        if (meanings.Count == 0)
        {
            return botClient.SendTextMessageAsync(
                update.CallbackQuery.Message.Chat.Id,
                $"There is no saved word: {callback.Data}");
        }

        _repository.RemoveWord(callback.UserId.AsRepositoryId(), callback.Data);

        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            $"Removed:\n{callback.Data} - {string.Join(',', meanings)}");
    }
}