namespace Blog.EventSourcing.Domain
{
    using System;

    public class PersonCreated : IPersonEvent
    {
        public PersonCreated(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            EMail = email;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string EMail { get; }
    }
}