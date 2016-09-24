namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Blog.EventSourcing.Domain.Events;

    using EventStore.ClientAPI;
    using EventStore.Common.Utils;
    using EventStore.Projections.Core.Utils;

    using Newtonsoft.Json;

    public class PersonRepository
    {
        private readonly IEventStoreConnection _store;

        public PersonRepository(IStoreFactory storeFactory)
        {
            _store = storeFactory.GetStore();
        }

        public string StreamName => "Person";

        public async Task<WriteResult> ActOn(Guid id, Guid commitId, Action<Person> act)
        {
            var person = Load(id);
            act(person);
            return await _store.AppendToStreamAsync(GetStreamName(id), ExpectedVersion.Any, GetEventData(person));
        }

        private IEnumerable<EventData> GetEventData(Person person)
        {
            return person.UncommitEvents.Select(GetEventData);
        }

        private EventData GetEventData(IPersonEvent evt)
        {
            var typeName = evt.GetType().Name;
            typeName = ToCamelCase(typeName);
            var dict = new Dictionary<string, object> { { "NetTypeName", evt.GetType().FullName } };
            return new EventData(Guid.NewGuid(), typeName, true, evt.ToJsonBytes(), dict.ToJsonBytes());
        }

        private string ToCamelCase(string str)
        {
           return str.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + str.Substring(1);
        }

        private Person Load(Guid id)
        {
            var evts = GetEvents(id).ToList();
            var person = new Person(evts);
            return person;
        }

        private string GetStreamName(Guid id)
        {
            return $"Person-{id}";
        }

        private IEnumerable<IPersonEvent> GetEvents(Guid id)
        {
            using (var stream = _store.ReadStreamEventsForwardAsync(GetStreamName(id), StreamPosition.Start, 4096, true))
            {
                var items = stream.Result;
                foreach (var resolvedEvent in items.Events)
                {
                    var metaData = resolvedEvent.Event.Metadata.ParseJson<Dictionary<string, object>>();

                    var type = Type.GetType((string)metaData["netTypeName"]);
                    if (type == null)
                    {
                        throw new InvalidOperationException($"Could not resolve type {metaData["NetTypeName"]}");
                    }

                    var t3 = resolvedEvent.Event.Data.FromUtf8();
                    var evt = JsonConvert.DeserializeObject(t3, type);
                    if ((evt is IPersonEvent) == false)
                    {
                        throw new InvalidOperationException($"Non IPersonEvent in personstream {type.FullName}");
                    }

                    yield return evt as IPersonEvent;
                }
            }
        }
    }
}