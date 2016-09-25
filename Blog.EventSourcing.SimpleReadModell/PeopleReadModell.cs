namespace Blog.EventSourcing.SimpleReadModell
{
    using System;
    using System.Collections.Generic;

    using Blog.EventSourcing.Events.Person;

    public class EmailChangedEventAndDate
    {
        public EmailChangedEventAndDate()
        {
            
        }

        public EmailChangedEventAndDate(IPersonEmailChanged evt, DateTime created)
        {
            PersonId = evt.Id;
            NewEmail = evt.Email;
            CreatedAt = created;
        }

        public Guid PersonId { get; set; }

        public string NewEmail { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class EmailChangesReadModell
    {
        private List<EmailChangedEventAndDate> _eventAndDate = new List<EmailChangedEventAndDate>();

        public IEnumerable<EmailChangedEventAndDate> EventAndDate => _eventAndDate;

        public void Handle(IPersonEvent evt, DateTime created)
        {
            var changed = evt as IPersonEmailChanged;
            if (changed != null)
            {
                _eventAndDate.Add(new EmailChangedEventAndDate(changed, created));
            }
        }

    }

    public class PeopleReadModell : IPeopleReadModell
    {
        private readonly Dictionary<Guid, NameAndEmail> _namesAndEmailsDictionary = new Dictionary<Guid, NameAndEmail>();

        public IEnumerable<NameAndEmail> NameAndEmails => _namesAndEmailsDictionary.Values;

        public void Handle(IPersonEvent evt)
        {
            var created = evt as PersonCreated;
            if (created != null)
            {
                Process(created);
            }

            var changed = evt as PersonNameChanged;
            if (changed != null)
            {
                Process(changed);
            }

            var emailChanged = evt as IPersonEmailChanged;
            if (emailChanged != null)
            {
                Process(emailChanged);
            }
        }

        private void Process(PersonCreated evt)
        {
            _namesAndEmailsDictionary.Add(evt.Id, new NameAndEmail() {Email = evt.Email, Name = evt.Name, PersonId = evt.Id});
        }

        private void Process(PersonNameChanged evt)
        {
            _namesAndEmailsDictionary[evt.Id].Name = evt.Name;
        }

        private void Process(IPersonEmailChanged evt)
        {
            _namesAndEmailsDictionary[evt.Id].Name = evt.Email;
        }

    }
}