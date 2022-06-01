using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore;

public record struct BotInstruments(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken);

public record struct BotMessage(string Text, long ChatId, long UserId, string UserName);

public record struct BotCallback(string Text, long ChatId);