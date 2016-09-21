namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Collections.Generic;

    public class Person
    {
        public Person(IEnumerable<IPersonEvent> events)
        {
            foreach (var personEvent in events)
            {
                ApplyChange(personEvent);
            }
        }

        public IEnumerable<IPersonEvent> UncommitEvents { get; } = new List<IPersonEvent>();

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public void Create(CreatePerson createPerson)
        {
            if (Id != Guid.Empty)
            {
                throw new InvalidOperationException("Cannot create on an already existing Person");
            }

            if (string.IsNullOrEmpty(createPerson.Name))
            {
                throw new ArgumentException("Name is empty");
            }

            if (string.IsNullOrEmpty(createPerson.Email))
            {
                throw new ArgumentException("Email is empty");
            }

            var evt = new PersonCreated(createPerson.PersonId, createPerson.Name, createPerson.Email);
            ApplyChange(evt);
        }

        public void ChangeName()
        {
            
        }

        private void ApplyChange(IPersonEvent personEvent)
        {
            Apply((dynamic)personEvent);
        }

        private void Apply(PersonCreated @event)
        {
            Name = @event.Name;
            Email = @event.EMail;
            Id = @event.Id;
        }
    }
}