using System;
using System.Collections.Generic;

namespace Vocabulary.Database.Entities
{
    internal sealed class Word
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public User User { get; init; }

        public string Data { get; init; }

        public IReadOnlyList<Meaning> Meanings { get; init; }
    }
}
