using System;
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

            _repository.AddWordWithMeaning(callback.UserId.AsRepositoryId(), keyValue[0], keyValue[1]);

            var allMeanings = string.Join(Environment.NewLine, _repository.FindMeanings(callback.UserId.AsRepositoryId(), keyValue[0]));

            return botClient.SendMessage(
                $"For word {keyValue[0]} added meaning {keyValue[1]}\nAll meanings:\n{allMeanings}",
                update.CallbackQuery);
        }
    }
}
