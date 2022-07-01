using System;

namespace Vocabulary.Database.Entities
{
    internal sealed class Meaning
    {
        public Meaning(Word word, string data)
        {
            Id = Guid.NewGuid();
            Word = word;
            Data = data;
        }

        public Guid Id { get; set; }

        public Word Word { get; set; }

        public string Data { get; set; }
    }
}
