using System;
using System.Linq;
using System.Threading.Tasks;
using BotCore.Cache;
using Vocabulary;

namespace BotCore.Messaging.Callbacks
{
    internal class AddWordMeaningCallbackHandler : CallbackHandlerBase
    {
        private readonly IVocabularyRepository _repository;

        public AddWordMeaningCallbackHandler(IDataCache dataCache, IVocabularyRepository repository) : base(dataCache)
        {
            _repository = repository;
        }

        public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.AddWordMeaning;

        protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
        {
            var (botClient, update, _) = botInstruments;

            var keyValue = callback.Data.Split('_');
            if (keyValue.Length != 2)
            {
                return botClient.SendMessage(
                    "Invalid formatting! Pair should be formatted as <code>word <b>_</b> meaning</code>",
                    update.CallbackQuery);
            }

            var word = keyValue[0].AsRepositoryString();
            var meaning = keyValue[1].AsRepositoryString();
            _repository.AddWordWithMeaning(callback.UserId.AsRepositoryId(), word, meaning);

            var allMeanings = string.Join(Environment.NewLine, _repository.UserVocabulary(callback.UserId.AsRepositoryId()).Single(v => v.Word == word).Meanings);

            return botClient.SendMessage(
                $"Added: <b>{word} - {meaning}</b>\nAll meanings:\n<b>{allMeanings}</b>",
                update.CallbackQuery);
        }
    }
}
