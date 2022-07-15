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

        public void AddWordWithMeaning(UserId userId, string word, string meaning)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var wordLowered = word.AsRepositoryString();
            var meaningLowered = meaning.AsRepositoryString();

            var dbWord = context.Words.FirstOrDefault(w => w.Data == wordLowered);

            if (dbWord == null)
            {
                dbWord = new Word { User = user, Data = wordLowered };
                context.Words.Add(dbWord);
            }

            var dbMeaning = context.Meanings.FirstOrDefault(m => m.Word == dbWord && m.Data == meaningLowered);
            if (dbMeaning == null)
            {
                dbMeaning = new Meaning { Word = dbWord, Data = meaningLowered };
                context.Meanings.Add(dbMeaning);
            }
            context.SaveChanges();
        }

        public void AddText(UserId userId, string text)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            context.Texts.Add(new Text { User = user, Data = text });
            context.SaveChanges();
        }

        public void RemoveWord(UserId userId, string word)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var wordLowered = word.AsRepositoryString();

            var dbWord = context.Words.FindOrThrow(
                w => w.User == user && w.Data == wordLowered,
                $"Word {word} not defined by user!");

            context.Words.Remove(dbWord);
            context.SaveChanges();
        }

        public void RemoveMeaning(UserId userId, string word, string meaning)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            var wordLowered = word.AsRepositoryString();
            var meaningLowered = meaning.AsRepositoryString();

            var dbWord = context.Words.FindOrThrow(
                w => w.User == user && w.Data == wordLowered,
                $"Word {wordLowered} not defined by user!");

            var dbMeaning = dbWord.Meanings.FindOrThrow(
                m => m.Data == meaningLowered,
                $"Meaning {meaningLowered} not defined for word {wordLowered}!");

            context.Meanings.Remove(dbMeaning);
            context.SaveChanges();
        }

        public IReadOnlyList<WordWithMeanings> UserVocabulary(UserId userId)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);

            return context.Words.Where(w => w.User == user)
                .Select(w => new WordWithMeanings(w.Data, w.Meanings.Select(m => m.Data).ToList()))
                .ToList();
        }

        public IReadOnlyList<string> Texts(UserId userId)
        {
            using var context = _dataContextProvider.Create();
            var user = EnsureUser(userId, context);
            return context.Texts.Where(t => t.User == user).Select(t => t.Data).ToList();
        }

        private static User EnsureUser(UserId userId, DataContext context)
        {
            var (userSource, id) = userId;
            var user = context.Users.FirstOrDefault(u => u.Source == userSource && u.SourceId == id);
            if (user == null)
            {
                user = new User { Source = userSource, SourceId = id };
                context.Users.Add(user);
                context.SaveChanges();
            }
            return user;
        }
    }
}