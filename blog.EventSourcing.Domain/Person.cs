namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Collections.Generic;

    using Blog.EventSourcing.Domain.Commands;
    using Blog.EventSourcing.Domain.Events;

    public class Person
    {
        private List<IPersonEvent> _uncommitedEvents = new List<IPersonEvent>();

        public Person(IEnumerable<IPersonEvent> events)
        {
            Version = 0;
            foreach (var personEvent in events)
            {
                Apply(personEvent);
                Version++;
            }
        }

        public IEnumerable<IPersonEvent> UncommitEvents => _uncommitedEvents;

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Version { get; }

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

            AssertEmailIsValid(createPerson.Email);

            var evt = new PersonCreated(createPerson.PersonId, createPerson.Name, createPerson.Email, DateTime.Now);
            ApplyChange(evt);
        }

        public void CorrectEmail(CorrectPersonEmail correctPersonEmail)
        {
            if (correctPersonEmail.PersonId != Id)
            {
                throw new InvalidOperationException("Command route error");
            }

            if (CreatedAt.AddDays(10) < DateTime.Now)
            {
                throw new InvalidOperationException("Not allowed to correct email after more then 10 days.");
            }

            AssertEmailIsValid(correctPersonEmail.Email);

            var evt = new PersonEmailCorrected(Id, correctPersonEmail.Email);
            ApplyChange(evt);
        }

        public void ChangeEmail(ChangePersonEmail changePersonEmail)
        {
            if (changePersonEmail.PersonId != Id)
            {
                throw new InvalidOperationException("Command route error");
            }

            AssertEmailIsValid(changePersonEmail.Email);

            var evt = new PersonEmailChanged(Id, changePersonEmail.Email);
            ApplyChange(evt);
        }

        public void ChangePersonName(ChangePersonName changePersonName)
        {
            if (changePersonName.PersonId != Id)
            {
                throw new InvalidOperationException("Command route error");
            }

            var evt = new PersonNameChanged(Id, changePersonName.Name);
            ApplyChange(evt);
        }

        private void Apply(IPersonEvent personEvent)
        {
            When((dynamic)personEvent);
        }

        private void ApplyChange(IPersonEvent personEvent)
        {
            Apply(personEvent);
            _uncommitedEvents.Add(personEvent);
        }

        private static void AssertEmailIsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email is empty");
            }
        }

        private void When(PersonCreated @event)
        {
            Name = @event.Name;
            Email = @event.Email;
            Id = @event.Id;
            CreatedAt = @event.CreatedAt;
        }

        private void When(IPersonEmailChanged @event)
        {
            Email = @event.Email;
        }

        private void When(PersonNameChanged @event)
        {
            Name = @event.Name;
        }
    }
}