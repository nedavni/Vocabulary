using System;

namespace Vocabulary.Database.Entities
{
    internal sealed class User
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public UserSource Source { get; init; }

        public string SourceId { get; init; }
    }
}
