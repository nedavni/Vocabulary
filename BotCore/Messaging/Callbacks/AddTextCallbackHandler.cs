using System.Threading.Tasks;
using BotCore.Cache;
using Vocabulary;

namespace BotCore.Messaging.Callbacks
{
    internal class AddTextCallbackHandler : CallbackHandlerBase
    {
        private readonly IVocabularyRepository _repository;

        public AddTextCallbackHandler(IDataCache cache, IVocabularyRepository repository) : base(cache)
        {
            _repository = repository;
        }

        public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.AddText;

        protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
        {
            _repository.Add(callback.UserId.AsRepositoryId(), callback.Data);
            var (client, update, _) = botInstruments;
            return client.SendMessage($"Added as text:\n\n{callback.Data}", update.CallbackQuery);
        }
    }
}
