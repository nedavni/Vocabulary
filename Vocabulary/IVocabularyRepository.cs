using System.Collections.Generic;

namespace Vocabulary
{
    public interface IVocabularyRepository
    {
        public void Add(UserId userId, string word, string meaning);

        public void Add(UserId userId, string text);

        public bool RemoveWord(UserId userId, string word);

        public void RemoveMeaning(UserId userId, string word, string meaning);

        IReadOnlyCollection<string> FindMeanings(UserId userId, string word);

        IReadOnlyCollection<string> FindWordsWithMeaning(UserId userId, string meaning);

        //TODO: provide text id for faster operations
        IReadOnlyCollection<string> FindTextThatContains(UserId asUserId, string callbackData);
        IReadOnlyList<WordWithMeanings> WordsWithMeanings(UserId repositoryId);
    }
}
