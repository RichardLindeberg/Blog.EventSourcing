using System.Collections.Generic;
using Blog.EventSourcing.Events.Person;

namespace Blog.EventSourcing.SimpleReadModell
{
    public interface IPeopleReadModell
    {
        IEnumerable<NameAndEmail> NameAndEmails { get; }

        void Handle(IPersonEvent evt);
    }
}