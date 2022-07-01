using System;

namespace Vocabulary.Database.Entities
{
    internal sealed class User
    {
        public User(UserSource source, string sourceId)
        {
            Id = Guid.NewGuid();
            Source = source;
            SourceId = sourceId;
        }

        public Guid Id { get; init; }

        public UserSource Source { get; init; }

        public string SourceId { get; init; }
    }
}
