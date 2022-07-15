using System;

namespace Vocabulary.Database.Entities
{
    internal sealed class Meaning
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public Word Word { get; init; }

        public string Data { get; init; }
    }
}
