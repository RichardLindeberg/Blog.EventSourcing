namespace Blog.EventSourcing.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Blog.EventSourcing.Events.Person;

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
            return person.UncommitEvents.Select(t => t.ToEventData());
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
                    yield return resolvedEvent.Event.ToIPersonEvent();
                }
            }
        }
    }


    public static class EventDataSerializer
    {
        public static EventData ToEventData(this IPersonEvent evt)
        {
            var typeName = evt.GetType().Name;
            typeName = ToCamelCase(typeName);
            var dict = new Dictionary<string, object> { { "NetTypeName", evt.GetType().FullName } };
            return new EventData(Guid.NewGuid(), typeName, true, evt.ToJsonBytes(), dict.ToJsonBytes());
        }

        public static IPersonEvent ToIPersonEvent(this RecordedEvent eventData)
        {
            var metaData = eventData.Metadata.ParseJson<Dictionary<string, object>>();

            var type = GetTypeWithAssemblyName((string)metaData["netTypeName"]);
            
            var evtDataAsString = eventData.Data.FromUtf8();
            var evt = JsonConvert.DeserializeObject(evtDataAsString, type);

            if ((evt is IPersonEvent) == false)
            {
                throw new InvalidOperationException($"Non IPersonEvent in personstream {type.FullName}");
            }

            return evt as IPersonEvent;
        }

        private static Type GetTypeWithAssemblyName(string assemblyQualifiedName)
        {
            return Type.GetType($"{assemblyQualifiedName}, Blog.EventSourcing.Events", true);
        }

        private static string ToCamelCase(string str)
        {
            return str.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + str.Substring(1);
        }

    }
}