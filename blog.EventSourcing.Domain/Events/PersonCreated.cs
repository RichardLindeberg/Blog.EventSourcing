namespace Blog.EventSourcing.Domain.Events
{
    using System;

    public class PersonCreated : IPersonEvent
    {
        public PersonCreated(Guid id, string name, string email, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Email = email;
            CreatedAt = createdAt;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Email { get; }

        public DateTime CreatedAt { get; }
    }
}