using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore.Handlers.Callback
{
    [Export(typeof(ICallbackHandler))]
    internal class AddWordMeaningCallbackHandler : CallbackHandlerBase
    {
        public AddWordMeaningCallbackHandler(IDataCache dataCache) : base(dataCache)
        {
        }

        public override CallbackKind CanHandleKind => CallbackKind.AddWordMeaning;

        protected override Task HandleInternal(CallbackData callback, BotInstruments botInstruments)
        {
            var (botClient, update, _) = botInstruments;
            var keyValue = callback.Data.Split('_');
            if (keyValue.Length != 2)
            {
                return botClient.SendTextMessageAsync(
                    update.CallbackQuery.Message.Chat.Id,
                    "Invalid formatting! Pair should be formatted as <code>word <b>_</b> meaning</code>",
                    ParseMode.Html);
            }

            //TODO: list of all meanings
            return botClient.SendTextMessageAsync(
                update.CallbackQuery.Message.Chat.Id,
                $"For word {keyValue[0]} added meaning {keyValue[1]}",
                ParseMode.Html);
        }
    }
}
