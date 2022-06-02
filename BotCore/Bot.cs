using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BotCore.Messaging;
using BotCore.Messaging.Callbacks;
using BotCore.Messaging.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotCore;

[Export]
public sealed class Bot
{
    private readonly TelegramBotClient _bot;
    private readonly IMessageHandler<BotMessage>[] _messageHandlers;
    private readonly IMessageHandler<UserCallback>[] _callbackHandlers;

    public Bot(string token, IEnumerable<IMessageHandler<BotMessage>> messageHandlers, IEnumerable<IMessageHandler<UserCallback>> callbackHandlers)
    {
        _bot = new TelegramBotClient(token);
        _messageHandlers = messageHandlers.ToArray();
        _callbackHandlers = callbackHandlers.ToArray();
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
        var instruments = new BotInstruments(botClient, update, cancellationToken);
        switch (update.Type)
        {
            case UpdateType.Message:
            case UpdateType.EditedMessage:
                await ProcessMessage(update.GetMessage(), instruments);
                break;
            case UpdateType.CallbackQuery:
                await ProcessCallback(instruments);
                break;
            default:
                throw new InvalidOperationException($"Invalid message type: {update.Type}");
        }
    }

    private async Task ProcessMessage(BotMessage message, BotInstruments instruments)
    {
        var handler = _messageHandlers.FirstOrDefault(h => h.CanHandle(message));

        if (handler == null)
        {
            throw new NotImplementedException($"Handlers for message {message.Text} not found!");
        }

        await handler.Handle(message, instruments);
    }

    private async Task ProcessCallback(BotInstruments botInstruments)
    {
        var answer = JsonSerializer.Deserialize<UserCallback>(botInstruments.Update.CallbackQuery.Data);

        var handler = _callbackHandlers.FirstOrDefault(h => h.CanHandle(answer));

        if (handler == null)
        {
            throw new NotImplementedException($"Handlers for message {answer} not found!");
        }

        await handler.Handle(answer, botInstruments);
    }
}