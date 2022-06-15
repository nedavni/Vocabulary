using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BotCore.Cache;
using BotCore.Messaging.Callbacks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotCore.Messaging.Messages;

[Export(typeof(IMessageHandler<BotMessage>))]
internal class TrainTranslationMessageHandler : CallbackHandlerBase, IMessageHandler<BotMessage>
{
    private readonly IDataCache _cache;

    public TrainTranslationMessageHandler(IDataCache cache) : base(cache)
    {
        _cache = cache;
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.CorrectMeaningChosen ||
                                                             callback.Kind == CallbackKind.WrongMeaningChosen;

    public bool CanHandle(BotMessage message) => message.Kind == MessageKind.TrainTranslation;

    public Task Handle(BotMessage message, BotInstruments instruments)
    {
        var word = "OloloWord " + Guid.NewGuid();
        var answerOptions = Enumerable.Range(1, 3).Select(i => $"Meaning{i}").ToArray();
        return MessageWithTranslationOptions(word, answerOptions, message.UserId, message.ChatId, instruments);
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

        var (word, meanings) = JsonSerializer.Deserialize<WrongTranslation>(callback.Data);
        await MessageWithTranslationOptions(word, meanings, callback.UserId, update.CallbackQuery.Message.Chat.Id, instruments);
    }

    private async Task HandleCorrectTranslation(CallbackData callback, BotInstruments instruments)
    {
        // 1) print all meanings for word
        var _ = JsonSerializer.Deserialize<CorrectTranslation>(callback.Data);
        // 2) go to next word

        var (botClient, update, _) = instruments;

        await instruments.BotClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            "Good job!\n Go to next word");

        var word = "OloloWord " + Guid.NewGuid();
        var answerOptions = Enumerable.Range(1, 3).Select(i => $"Meaning{i}").ToArray();

        await MessageWithTranslationOptions(word, answerOptions, callback.UserId, update.CallbackQuery.Message.Chat.Id, instruments);
    }

    private Task MessageWithTranslationOptions(string word, IReadOnlyList<string> answerOptions, long userId, long chatId,BotInstruments instruments)
    {
        var reply = answerOptions.Select((translation, i) =>
        {
            if (i+1 == 2)
            {
                return new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        translation,
                        _cache.StoreCallback(CallbackKind.CorrectMeaningChosen, userId, JsonSerializer.Serialize(new CorrectTranslation(word))).Serialize())

                };
            }

            return new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    translation,
                    _cache.StoreCallback(CallbackKind.WrongMeaningChosen, userId, JsonSerializer.Serialize(new WrongTranslation(word, answerOptions))).Serialize())
            };
        });
        InlineKeyboardMarkup inlineKeyboard = new(reply);

        return instruments.BotClient.SendTextMessageAsync(
            chatId,
            word,
            replyMarkup: inlineKeyboard);
    }

    private record struct WrongTranslation(string Word, IReadOnlyList<string> Meanings);

    private record struct CorrectTranslation(string Word);
}