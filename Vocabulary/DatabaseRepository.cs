using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Vocabulary.Database;
using Vocabulary.Database.Entities;

namespace Vocabulary
{
    [Export(typeof(IVocabularyRepository))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class DatabaseRepository : IVocabularyRepository
    {
        private readonly IDataContextProvider _dataContextProvider;

        public DatabaseRepository(IDataContextProvider dataContextProvider)
        {
            _dataContextProvider = dataContextProvider;
        }

        public void Add(UserId userId, string word, string meaning)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var dbWord = new Word(user, word);
            context.Words.Add(dbWord);

            var dbMeaning = new Meaning(dbWord, meaning);
            context.Meanings.Add(dbMeaning);

            context.SaveChanges();
        }

        public void Add(UserId userId, string text)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            context.Texts.Add(new Text(user, text));
            context.SaveChanges();
        }

        public void RemoveWord(UserId userId, string word)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var dbWord = context.Words.FindOrThrow(
                w => w.User == user && w.Data == word,
                $"Word {word} not defined by user!");

            context.Words.Remove(dbWord);
            context.SaveChanges();
        }

        public void RemoveMeaning(UserId userId, string word, string meaning)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var dbWord = context.Words.FindOrThrow(
                w => w.User == user && w.Data == word,
                $"Word {word} not defined by user!");

            var dbMeaning = dbWord.Meanings.FindOrThrow(
                m => m.Data == meaning,
                $"Meaning {meaning} not defined for word {word}!");

            context.Meanings.Remove(dbMeaning);
            context.SaveChanges();
        }

        public IReadOnlyCollection<string> FindMeanings(UserId userId, string word)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var dbWord = context.Words.FindOrThrow(
                w => w.User == user && w.Data == word,
                $"Word {word} not defined by user!");
            return dbWord.Meanings.Select(m => m.Data).ToList();
        }

        public IReadOnlyCollection<string> FindWordsWithMeaning(UserId userId, string meaning)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var dbWords = context.Meanings.Where(m => m.Word.User == user && m.Data == meaning).Select(m => m.Word);
            return dbWords.Select(w => w.Data).ToList();
        }

        public IReadOnlyCollection<string> FindTextThatContains(UserId userId, string words)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            return context.Texts.Where(t => t.User == user && t.Data.Contains(words)).Select(t => t.Data).ToList();
        }

        public IReadOnlyList<WordWithMeanings> UserVocabulary(UserId userId)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            return context.Words.Where(w => w.User == user)
                .Select(w => new WordWithMeanings(w.Data, w.Meanings.Select(m => m.Data).ToList()))
                .ToList();
        }

        private static User EnsureUser(UserId userId, DataContext context)
        {
            var (userSource, id) = userId;
            var user = context.Users.FirstOrDefault(u => u.Source == userSource && u.SourceId == id);
            if (user == null)
            {
                user = new User(userSource, id);
                context.Users.Add(user);
                context.SaveChanges();
            }
            return user;
        }
    }
}