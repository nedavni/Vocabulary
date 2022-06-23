using System;
using System.Collections.Generic;
using System.Linq;

namespace Vocabulary
{
    internal sealed class InMemoryRepository : IVocabularyRepository
    {
        public void Add(UserId userId, string word, string meaning)
        {
            throw new NotImplementedException();
        }

        public void Add(UserId userId, string text)
        {
            throw new NotImplementedException();
        }

        public bool RemoveWord(UserId userId, string word)
        {
            throw new NotImplementedException();
        }

        public void RemoveMeaning(UserId userId, string word, string meaning)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<string> FindMeanings(UserId userId, string word)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<string> FindWordsWithMeaning(UserId userId, string meaning)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<string> FindTextThatContains(UserId asUserId, string callbackData)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<WordWithMeanings> WordsWithMeanings(UserId repositoryId)
        {
            throw new NotImplementedException();
        }
    }
}