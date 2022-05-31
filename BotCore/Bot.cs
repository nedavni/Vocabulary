using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BotCore.Handlers.Callback;
using BotCore.Handlers.Message;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore;

public sealed class Bot
{
    private readonly TelegramBotClient _bot;
    private readonly IMessageHandler[] _messageHandlers;
    private readonly ICallbackHandler[] _callbackHandlers;

    public Bot(string token)
    {
        _bot = new TelegramBotClient(token);
        _messageHandlers = new IMessageHandler[]
        {
            new StartMessageHandler(),
            new MenuMessageHandler(),
            new UnknownMessageHandler()
        }
            .OrderBy(h => h.Order)
            .ToArray();

        _callbackHandlers = new ICallbackHandler[]
        {
            new MenuCallbackHandler(),
            new AddTextCallbackHandler(),
            new AddKeyValueCallbackHandler()
        };
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
        switch (update.Type)
        {
            case UpdateType.Message:
                await ProcessMessage(botClient, update, cancellationToken);
                break;
            case UpdateType.CallbackQuery:
                await ProcessCallback(botClient, update, cancellationToken);
                break;
            default:
                throw new InvalidOperationException($"Invalid message type: {update.Message.Type}");
        }
    }

    private async Task ProcessMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handler = _messageHandlers.FirstOrDefault(h => h.CanHandle(update.Message.Text));

        if (handler == null)
        {
            throw new NotImplementedException($"Handlers for message {update.Message.Text} not found!");
        }

        await handler.Handle(update.Message.Text, botClient, update, cancellationToken);
    }

    private async Task ProcessCallback(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var answer = JsonSerializer.Deserialize<UserCallback>(update.CallbackQuery.Data)!;

        var handler = _callbackHandlers.FirstOrDefault(h => h.CanHandle(answer));

        if (handler == null)
        {
            throw new NotImplementedException($"Handlers for message {update.Message.Text} not found!");
        }

        await handler.Handle(answer, botClient, update, cancellationToken);
    }
}