using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Vocabulary;

namespace BotCore
{
    internal static class BotExtensions
    {
        public static UserId AsRepositoryId(this long id) => new(UserSource.Telegram, id.ToString());

        public static Task SendMessage(this ITelegramBotClient client, string message, long chatId, IReplyMarkup markup = default, CancellationToken token = default)
            => client.SendTextMessageAsync(chatId, message, ParseMode.Html, replyMarkup: markup, cancellationToken: token);

        public static Task SendMessage(this ITelegramBotClient client, string message, CallbackQuery callback, IReplyMarkup markup = default, CancellationToken token = default)
            => client.SendTextMessageAsync(callback.Message.Chat.Id, message, ParseMode.Html, replyMarkup: markup, cancellationToken: token);
    }
}
