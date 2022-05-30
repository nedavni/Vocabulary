namespace Vocabulary
{
    public enum UserSource
    {
        Telegram = 0,
        WebApp
    }

    public sealed record UserId(UserSource Source, string Id);
}