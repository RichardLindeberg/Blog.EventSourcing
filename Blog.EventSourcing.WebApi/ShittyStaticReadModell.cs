namespace Blog.EventSourcing.WebApi
{
    using System;

    using Blog.EventSourcing.Domain;
    using Blog.EventSourcing.SimpleReadModell;

    using EventStore.ClientAPI;

    public class ShittyStaticReadModell
    {
        public static Lazy<PeopleReadModell> People = new Lazy<PeopleReadModell>(() => StartPeopleModell());

        public static EMailChangesReadModell emailChanges = StartEmailChangedModell();

        private static EMailChangesReadModell StartEmailChangedModell()
        {
            var store = new StoreFactory().GetStore();

            var modell = new EMailChangesReadModell();

            var s = store.SubscribeToStreamFrom(
                "people",
                null,
                CatchUpSubscriptionSettings.Default,
                (subscription, @event) => modell.Handle(@event.Event.ToIPersonEvent(), @event.Event.Created),
                subscription => Console.WriteLine("Started catchup"),
                (subscription, reason, arg3) => Console.WriteLine("Dropped subscription"));
            return modell;

        }

        private static PeopleReadModell StartPeopleModell()
        {
            var store = new StoreFactory().GetStore();
          
            var peopleModell = new PeopleReadModell();
                
            var s = store.SubscribeToStreamFrom(
                "people",
                null,
                CatchUpSubscriptionSettings.Default,
                (subscription, @event) => peopleModell.Handle(@event.Event.ToIPersonEvent()),
                subscription => Console.WriteLine("Started catchup"),
                (subscription, reason, arg3) => Console.WriteLine("Dropped subscription"));
            return peopleModell;
        }
    }
}