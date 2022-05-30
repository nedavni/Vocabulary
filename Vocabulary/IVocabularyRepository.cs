namespace Vocabulary
{
    public interface IVocabularyRepository
    {
        public void Add(UserId userId, string word, string meaning);

        public void Add(UserId userId, string text);

        public void RemoveWord(UserId userId, string word);

        public void RemoveMeaning(UserId userId, string word, string meaning);
    }
}
