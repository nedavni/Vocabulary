using System.Collections.Generic;

namespace Vocabulary
{
    public interface IVocabularyRepository
    {
        public void AddWordWithMeaning(UserId userId, string word, string meaning);

        public void AddText(UserId userId, string text);

        public void RemoveWord(UserId userId, string word);

        public void RemoveMeaning(UserId userId, string word, string meaning);

        IReadOnlyCollection<string> FindMeanings(UserId userId, string word);

        IReadOnlyCollection<string> FindWordsWithMeaning(UserId userId, string meaning);

        //TODO: provide text id for faster operations
        IReadOnlyCollection<string> FindTextThatContains(UserId asUserId, string words);

        IReadOnlyList<WordWithMeanings> UserVocabulary(UserId repositoryId);
    }
}
