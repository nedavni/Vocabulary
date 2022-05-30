using System;
using System.Collections.Generic;
using System.Linq;

namespace Vocabulary
{
    //internal sealed class InMemoryRepository : IVocabularyRepository
    //{
    //    private readonly IDictionary<VocabularyItem, HashSet<string>> _vocabulary;

    //    public InMemoryRepository()
    //    {
    //        _vocabulary = new Dictionary<VocabularyItem, HashSet<string>>();
    //    }

    //    public IEnumerable<string> UntouchedWords => _vocabulary.Keys.Where(w => !w.IsTouched).Select(w => w.Word);

    //    public IEnumerable<string> GetMeanings(string word) => _vocabulary[new VocabularyItem { Word = word }];

    //    public void Add(string word, string meaning)
    //    {
    //        var item = new VocabularyItem { Word = word };
    //        if (!_vocabulary.ContainsKey(item))
    //        {
    //            _vocabulary.Add(item, new HashSet<string> { meaning });
    //        }
    //        else
    //        {
    //            _vocabulary[item].Add(meaning);
    //        }
    //    }

    //    public void Add(string text)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RemoveWord(string word)
    //    {
    //        var item = new VocabularyItem { Word = word };
    //        if (_vocabulary.ContainsKey(item))
    //        {
    //            _vocabulary.Remove(item);
    //        }
    //    }

    //    public void RemoveMeaning(string word, string meaning)
    //    {
    //        var item = new VocabularyItem { Word = word };
    //        _vocabulary[item].Remove(meaning);
    //    }

    //    private sealed class VocabularyItem : IEquatable<VocabularyItem>
    //    {
    //        public string Word { get; init; }

    //        public bool IsTouched { get; init; }

    //        public bool Equals(VocabularyItem other)
    //        {
    //            if (ReferenceEquals(null, other))
    //                return false;
    //            if (ReferenceEquals(this, other))
    //                return true;
    //            return Word == other.Word;
    //        }

    //        public override bool Equals(object obj)
    //        {
    //            return ReferenceEquals(this, obj) || obj is VocabularyItem other && Equals(other);
    //        }

    //        public override int GetHashCode()
    //        {
    //            return (Word != null ? Word.GetHashCode() : 0);
    //        }
    //    }
    //}
}