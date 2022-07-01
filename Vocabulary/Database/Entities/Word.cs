using System;
using System.Collections.Generic;

namespace Vocabulary.Database.Entities
{
    internal sealed class Word
    {
        public Word(User user, string data)
        {
            Id = Guid.NewGuid();
            User = user;
            Data = data;
        }

        public Guid Id { get; set; }

        public User User { get; set; }

        public string Data { get; set; }

        public IReadOnlyList<Meaning> Meanings { get; set; }
    }
}
