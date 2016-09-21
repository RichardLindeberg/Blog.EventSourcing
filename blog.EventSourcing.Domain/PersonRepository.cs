namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Linq;

    using Blog.EventSourcing.Domain.Events;

    using NEventStore;

    public class PersonRepository
    {
        private readonly IStoreEvents _store;

        protected PersonRepository(IStoreFactory storeFactory)
        {
            _store = storeFactory.GetStore();
        }

        public string StreamName => "Person";

        public void ActOn(Guid id, Guid commitId, Action<Person> act)
        {
            using (var stream = _store.OpenStream(StreamName, id))
            {
                var events = stream.CommittedEvents.Select(t => t.Body).OfType<IPersonEvent>();

                var person = new Person(events);
                act(person);

                foreach (var @event in person.UncommitEvents)
                {
                    var evt = new EventMessage { Body = @event };
                    stream.Add(evt);
                }

                stream.CommitChanges(commitId);
            }
        }
    }
}