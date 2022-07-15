using System;

namespace Vocabulary.Database.Entities
{
    internal sealed class Text
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public User User { get; init; }

        public string Data { get; init; }
    }
}
