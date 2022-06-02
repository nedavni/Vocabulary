﻿namespace BotCore
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

    internal class BotCommands
    {
        // Menu
        public const string Train = nameof(Train);
        public const string Repeat = nameof(Repeat);
    }
}