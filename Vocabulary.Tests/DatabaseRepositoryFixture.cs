using System.Linq;
using NUnit.Framework;
using Vocabulary.Database;

namespace Vocabulary.Tests
{
    [TestFixture]
    public class DatabaseRepositoryFixture
    {
        private UserId _user;
        private IVocabularyRepository _repository;

        [SetUp]
        public void Setup()
        {
            _user = new UserId(UserSource.Telegram, "1");
            _repository = new DatabaseRepository(new InMemoryDatabaseContextProvider());
        }

        [Test]
        public void WhenAddWord_ItCanBeFound()
        {
            _repository.AddWordWithMeaning(_user, "woRd1", "Meaning1");
            _repository.AddWordWithMeaning(_user, "wOrd1", "MeanIng1");
            _repository.AddWordWithMeaning(_user, "Word1", "meaning2");

            var words = _repository.UserVocabulary(_user);

            Assert.That(words.Count, Is.EqualTo(1));
            Assert.That(words[0].Word.ToLowerInvariant(), Is.EqualTo("word1"));
            CollectionAssert.AreEquivalent(words[0].Meanings.Select(w => w.ToLowerInvariant()), new[] { "meaning1", "meaning2" });
        }

        [Test]
        public void WhenAddMeaning_ItShouldBeAssociatedWithWord()
        {
            AddWordWithMeanings("word1");
            AddWordWithMeanings("word2");

            VerifyMeanings("Word1");
            VerifyMeanings("WoRd2");

            void AddWordWithMeanings(string word)
            {
                _repository.AddWordWithMeaning(_user, word, "meanIng1");
                _repository.AddWordWithMeaning(_user, word, "meanIng2");
            }

            void VerifyMeanings(string word)
            {
                var meanings = _repository.UserVocabulary(_user).Single(v => v.Word == word.ToLowerInvariant()).Meanings;
                CollectionAssert.AreEquivalent(meanings, new[] { "meaning1", "meaning2" });
            }
        }

        [Test]
        public void WhenRemoveWord_MeaningsShouldBeRemoved()
        {
            var word = "word1";
            _repository.AddWordWithMeaning(_user, word, "meanIng1");
            _repository.AddWordWithMeaning(_user, word, "meanIng2");
            _repository.RemoveWord(_user, word);

            Assert.That(_repository.UserVocabulary(_user), Is.Empty);
        }

        [Test]
        public void WhenRemoveMeaning_ItShpuldBeRemoved()
        {
            var word = "word1";
            _repository.AddWordWithMeaning(_user, word, "meanIng1");
            _repository.AddWordWithMeaning(_user, word, "meanIng2");
            var meaningToRemove = _repository.UserVocabulary(_user).Single(w => w.Word == word).Meanings.First();
            _repository.RemoveMeaning(_user, word, meaningToRemove);

            var vocabulary = _repository.UserVocabulary(_user);
            Assert.That(vocabulary.Count, Is.EqualTo(1));
            Assert.That(vocabulary[0].Word, Is.EqualTo(word));
            Assert.That(vocabulary[0].Meanings.Count, Is.EqualTo(1));
            Assert.That(vocabulary[0].Meanings[0], Is.Not.EqualTo(meaningToRemove));
        }

        [Test]
        public void WhenWordWasNotAdded_RemoveWordShouldThrow()
        {

        }

        [Test]
        public void WhenRemoveMeaning_ItShouldBeRemoved()
        {

        }

        [Test]
        public void WhenMeaningWasNotAdded_RemoveMeaningShouldThrow()
        {

        }
    }
}
