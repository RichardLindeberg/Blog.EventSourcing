namespace Blog.EventSourcing.Events.Person
{
    using System;

    public class PersonNameChanged : IPersonEvent
    {
        public PersonNameChanged(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
    }
}