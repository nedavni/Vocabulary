using System.Collections.Generic;

namespace Vocabulary
{
    public interface IVocabularyRepository
    {
        /// <summary>
        /// Add word and meanings converted to lowercase using the casing rules of the invariant culture
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="word"></param>
        /// <param name="meaning"></param>
        void AddWordWithMeaning(UserId userId, string word, string meaning);

        void AddText(UserId userId, string text);

        void RemoveWord(UserId userId, string word);

        void RemoveMeaning(UserId userId, string word, string meaning);

        //IReadOnlyCollection<string> FindMeanings(UserId userId, string word);

        //IReadOnlyCollection<string> FindWordsByMeaning(UserId userId, string meaning);

        //TODO: provide text id for faster operations
        //IReadOnlyCollection<string> FindTextThatContains(UserId asUserId, string words);

        IReadOnlyList<WordWithMeanings> UserVocabulary(UserId userId);

        IReadOnlyList<string> Texts(UserId userId);
    }
}
