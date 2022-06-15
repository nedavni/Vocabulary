﻿using System.Threading.Tasks;
using BotCore.Cache;
using Telegram.Bot;

namespace BotCore.Messaging.Callbacks;

internal class RemoveExactTextCallbackHandler : CallbackHandlerBase
{
    public RemoveExactTextCallbackHandler(IDataCache cache) : base(cache)
    {
    }

    public override bool CanHandle(UserCallback callback) => callback.Kind == CallbackKind.RemoveExactText;

    protected override Task HandleInternal(CallbackData callback, CallbackKind kind, BotInstruments botInstruments)
    {
        var (botClient, update, _) = botInstruments;

        return botClient.SendTextMessageAsync(
            update.CallbackQuery.Message.Chat.Id,
            $"Text removed:\n {callback.Data}");    }
}