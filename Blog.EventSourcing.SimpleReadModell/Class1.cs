﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EventSourcing.SimpleReadModell
{
    using Blog.EventSourcing.Events.Person;

    public class NameAndEmail
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Guid PersonId { get; set; }
    }

    public class PeopleReadModell
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
