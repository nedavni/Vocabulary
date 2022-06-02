namespace BotCore.Messaging.Callbacks
{
    public enum CallbackKind
    {
        Menu = 0,
        AddWordMeaning,
        AddText,
        RemoveWord,
        RemoveMeaning,
        RemoveMeaningForWord,
        RemoveTextThatContainsWord,
        RemoveExactText
    }

    public static class KnownMenuCommands
    {

    }
}