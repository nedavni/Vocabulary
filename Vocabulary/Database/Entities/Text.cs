using System;

namespace Vocabulary.Database.Entities
{
    internal sealed class Text
    {
        public Text(User user, string data)
        {
            Id = Guid.NewGuid();
            User = user;
            Data = data;
        }
        
        public Guid Id { get; init; }

        public User User { get; init; }

        public string Data { get; init; }
    }
}
