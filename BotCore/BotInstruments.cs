using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotCore;

public record struct BotInstruments(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken);
