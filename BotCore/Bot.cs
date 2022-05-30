using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Vocabulary;

namespace BotCore;

public sealed class Bot
{
    private readonly TelegramBotClient _bot;
    private readonly IVocabularyRepository _repository;

    public Bot(string token)
    {
        _bot = new TelegramBotClient(token);
        //_repository = new InMemoryRepository();
    }


    public Task Start(CancellationToken stopBotToken)
    {
        return _bot.ReceiveAsync(OnUpdate, OnError, cancellationToken: stopBotToken);
    }

    private Task OnError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // TBD
        throw exception;
        return Task.FromException(exception);
    }

    private async Task OnUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(new string('-', 20));
        Console.WriteLine($"Update type: {update.Type}");
        Console.WriteLine($"Update message type: {update.Message?.Type}");
        Console.WriteLine($"Message from id: {update.Message?.From.Id}");
        Console.WriteLine($"Message text: {update.Message?.Text}");
        Console.WriteLine($"Message text: {update.Message?.ReplyToMessage?.Text}");
        Console.WriteLine(new string('-', 20));

        switch (update.Type)
        {
            case UpdateType.Message:
                await ProcessMessage(botClient, update);
                break;
            case UpdateType.CallbackQuery:
                await ProcessCallback(botClient, update);
                break;
            default:
                throw new InvalidOperationException($"Invalid message type: {update.Message.Type}");
        }
    }

    private async Task ProcessMessage(ITelegramBotClient botClient, Update update)
    {
        switch (update.Message.Text)
        {
            case "/start":
                await botClient.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    $"Hello {update.Message.From.Username}!\nYou can add pair of words using following formatting" +
                    " <code>word <b>_</b> meaning</code> or add a plain text. Just start typing :)",
                    ParseMode.Html);
                break;
        }

        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotCommands.Menu,
                    JsonSerializer.Serialize(new MenuAnswer(BotCommands.Menu, update.Message.From.Id,
                        update.Message.Text))),
                InlineKeyboardButton.WithCallbackData(BotCommands.AddText,
                    JsonSerializer.Serialize(new MenuAnswer(BotCommands.AddText, update.Message.From.Id,
                        update.Message.Text))),
                InlineKeyboardButton.WithCallbackData(BotCommands.AddWord,
                    JsonSerializer.Serialize(new MenuAnswer(BotCommands.AddWord, update.Message.From.Id,
                        update.Message.Text)))
            });

        await botClient.SendTextMessageAsync(
            update.Message.Chat.Id,
            update.Message.Text,
            replyMarkup: inlineKeyboard);
    }

    private async Task ProcessCallback(ITelegramBotClient botClient, Update update)
    {
        var answer = JsonSerializer.Deserialize<MenuAnswer>(update.CallbackQuery.Data)!;

        switch (answer.Command)
        {
            case BotCommands.Menu:
                {
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(
                        new[]
                        {
                            new KeyboardButton[] {BotCommands.Train, BotCommands.Repeat }
                        })
                    {
                        ResizeKeyboard = true
                    };

                    await botClient.SendTextMessageAsync(
                        update.CallbackQuery.Message.Chat.Id,
                        "/menu",
                        replyMarkup: replyKeyboardMarkup);
                    break;
                }
            case BotCommands.AddText:
                {
                    _repository.Add(new UserId(UserSource.Telegram, answer.UserId.ToString()), answer.Payload);
                    break;
                }
            case BotCommands.AddWord:
                {
                    var keyValue = answer.Payload.Split('_');
                    if (keyValue.Length != 2)
                    {
                        var message = await botClient.SendTextMessageAsync(
                            update.CallbackQuery.Message.Chat.Id,
                            "Invalid formatting! Pair should be formatted as <code>word <b>_</b> meaning</code>",
                            ParseMode.Html);
                    }

                    break;
                }
        }
    }

    private void SendInlineKeyboard(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken,
        long chatId)
    {
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            // first row
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1.1", "11"),
                InlineKeyboardButton.WithCallbackData("1.2", "12")
            },
            // second row
            new[]
            {
                InlineKeyboardButton.WithCallbackData("2.1", "21"),
                InlineKeyboardButton.WithCallbackData("2.2", "22")
            }
        });

        var sentMessage = botClient.SendTextMessageAsync(
            chatId,
            "A message with an inline keyboard markup",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken).Result;
    }

    private Task SendKeyboardMessage(ITelegramBotClient botClient, Update update)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] {"Train all", "Train touched"}
        })
        {
            ResizeKeyboard = true
        };

        return botClient.SendTextMessageAsync(
            update.Message?.Chat.Id,
            "uu",
            replyMarkup: replyKeyboardMarkup);
    }
}