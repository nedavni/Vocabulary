using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Handlers.Message;

[Export(typeof(IMessageHandler))]
internal class UnknownMessageHandler : IMessageHandler
{
    private readonly IDataCache _dataCache;

    public UnknownMessageHandler(IDataCache dataCache)
    {
        _dataCache = dataCache;
    }

    public int Order => int.MaxValue;

    public bool CanHandle(string message)
    {
        return true;
    }

    public Task Handle(BotMessage message, BotInstruments instruments)
    {
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Add as text",
                        _dataCache.StoreCallback(CallbackKind.AddText, message.UserId, message.Text).Serialize()),
                    InlineKeyboardButton.WithCallbackData(
                        "Add as word - meaning",
                        _dataCache.StoreCallback(CallbackKind.AddWordMeaning, message.UserId, message.Text).Serialize())
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Remove word",
                        _dataCache.StoreCallback(CallbackKind.RemoveWord, message.UserId, message.Text).Serialize()),
                    InlineKeyboardButton.WithCallbackData(
                        "Remove meaning",
                        _dataCache.StoreCallback(CallbackKind.RemoveMeaning, message.UserId, message.Text).Serialize()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Remove text that contains word",
                        _dataCache.StoreCallback(CallbackKind.RemoveTextThatContainsWord, message.UserId, message.Text).Serialize())
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Menu",
                        _dataCache.StoreCallback(CallbackKind.Menu, message.UserId, message.Text).Serialize())
                }
            });

        return instruments.BotClient.SendTextMessageAsync(
            message.ChatId,
            message.Text,
            replyMarkup: inlineKeyboard);
    }
}