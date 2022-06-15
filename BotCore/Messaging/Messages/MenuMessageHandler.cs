using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BotCore.Messaging.Callbacks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Messaging.Messages;

[Export(typeof(IMessageHandler<>))]
internal class MenuMessageHandler : IMessageHandler<BotMessage>, IMessageHandler<UserCallback>
{
    public bool CanHandle(BotMessage message) => message.Kind == MessageKind.Menu;

    public bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.Menu;

    public Task Handle(UserCallback callback, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;
        return ShowMenu(update.CallbackQuery.Message.Chat.Id, botClient);
    }


    public Task Handle(BotMessage message, BotInstruments instruments)
    {
        return ShowMenu(message.ChatId, instruments.BotClient);
    }

    private static Task ShowMenu(long chatId, ITelegramBotClient botClient)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { "/traintranslation", "Menu"}
            })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        return botClient.SendTextMessageAsync(
            chatId,
            "Menu",
            replyMarkup: replyKeyboardMarkup);
    }
}