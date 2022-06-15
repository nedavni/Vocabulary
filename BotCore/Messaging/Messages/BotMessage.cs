namespace BotCore.Messaging.Messages;

public record struct BotMessage(MessageKind Kind, string Text, long ChatId, long UserId, string UserName);

public enum MessageKind
{
    Start = 0,
    Menu,
    TrainTranslation,
    Unknown
}